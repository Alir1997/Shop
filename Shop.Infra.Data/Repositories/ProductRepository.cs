using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Site.Products;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region constractor
        private readonly ShopDbContext _context;
        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }


        #endregion

        #region product-admin
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckUrlNameCategory(string urlName)
        {
            return await _context.productCategories.AsQueryable().
                AnyAsync(c => c.UrlName == urlName);
        }

        public async Task AddProductCategory(ProductCategory productCategory)
        {
            await _context.productCategories.AddAsync(productCategory);
        }

        public async Task<ProductCategory> GetProductCategoryById(long id)
        {
            return await _context.productCategories.AsQueryable().
                SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CheckUrlNameCategory(string urlName, long categoryId)
        {
            return await _context.productCategories.AsQueryable().
                AnyAsync(c => c.UrlName == urlName && c.Id != categoryId);
        }

        public void UpdateProductCategory(ProductCategory Category)
        {
            _context.Update(Category);
        }

        public async Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            var query = _context.productCategories.AsQueryable();
            #region filter
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
            }
            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetProductCategories(allData);
        }
        #endregion

        #region product
        public async Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter)
        {
            var query = _context.Products.
                Include(c => c.ProductSelectedCategories).
                ThenInclude(c => c.ProductCategory).
                AsQueryable();

            #region filter

           
            if (!string.IsNullOrEmpty(filter.ProductName))
            {
          
                query = query.Where(c => EF.Functions.Like( c.Name, $"%{filter.ProductName}%") || c.ProductSelectedCategories.Any(s => EF.Functions.Like(s.ProductCategory.Title, $"%{filter.ProductName}%")));
            
            }


            if (!string.IsNullOrEmpty(filter.FilterByCategory))
            {
                query = query.Where(c => c.ProductSelectedCategories.Any(s => s.ProductCategory.UrlName == filter.FilterByCategory));

          
            }

            #endregion
            #region stste
            switch (filter.ProductState)
            {
                case ProductState.All:
                    query = query.Where(c => !c.IsDelete);
                    break;
                case ProductState.IsActive:
                    query = query.Where(c => c.IsActive);
                    break;
                case ProductState.Delete:
                    query = query.Where(c => c.IsDelete);
                    break;
            }

            switch (filter.ProductOrder)
            {
                case ProductOrder.All:
                    break;
                case ProductOrder.ProductNewss:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.CreateDate);
                    break;
                case ProductOrder.Productexp:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.Price);
                    break;
                case ProductOrder.ProductInExpensive:
                    query = query.Where(c => c.IsActive).OrderBy(c => c.Price);
                    break;
            }

            #region productBox
            switch (filter.ProductBox)
            {
                case ProductBox.Default:
                    break;
                case ProductBox.ItemBoxInSide:

                    var pagerBox = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
                    var allDataBox = await query.Paging(pagerBox).Select(c => new ProductItemViewModel
                    {
                        ProductCategory = c.ProductSelectedCategories.Select(c => c.ProductCategory).First(),
                        CommentCount = 0,
                        Price = c.Price,
                        ProductId = c.Id,
                        ProductImageName = c.ProductImageName,
                        ProductName = c.Name

                    }).ToListAsync();

                    return filter.SetPaging(pagerBox).SetProductsItem(allDataBox);
            }
            #endregion
            #endregion
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();
            return filter.SetPaging(pager).SetProducts(allData);


        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task RemoveProductSelectedCategories(long productId)
        {
            var allProductSelectedCategories = await _context.productSelectedCategories.AsQueryable().
                Where(c => c.ProductId == productId).ToListAsync();
            if (allProductSelectedCategories.Any())
            {
                _context.productSelectedCategories.RemoveRange(allProductSelectedCategories);
            }
        }

        public async Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId)
        {
            if (productSelectedCategories != null && productSelectedCategories.Any())
            {
                var newProductSelectedCategories = new List<ProductSelectedCategories>();

                foreach (var categoryId in productSelectedCategories)
                {
                    newProductSelectedCategories.Add(new ProductSelectedCategories
                    {
                        ProductId = productId,
                        ProductCategoryId = categoryId
                    });
                }

                await _context.productSelectedCategories.AddRangeAsync(newProductSelectedCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _context.productCategories.AsQueryable().
                Where(c => !c.IsDelete).
                ToListAsync();
        }
        public async Task<List<long>> GetAllProductCategoriesId(long productId)
        {
            return await _context.productSelectedCategories.AsQueryable().
                Where(c => !c.IsDelete && c.ProductId == productId).
                Select(c => c.Id).
                ToListAsync();
        }

        public async Task<Product> GetProductById(long productId)
        {
            return await _context.Products.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == productId);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable().
                Where(c => c.Id == productId).FirstOrDefaultAsync();
            if (currentProduct != null)
            {
                currentProduct.IsDelete = true;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable().
                Where(c => c.Id == productId).FirstOrDefaultAsync();
            if (currentProduct != null)
            {
                currentProduct.IsDelete = false;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddProductGalleries(List<ProductGalleries> productGalleries)
        {
            await _context.productGalleries.AddRangeAsync(productGalleries);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckProduct(long productId)
        {
            return await _context.Products.AsQueryable().
                AnyAsync(c => c.Id == productId);
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _context.productGalleries.AsQueryable().
                Where(c => c.ProductId == productId && !c.IsDelete)
                .ToListAsync();
        }

        public async Task<ProductGalleries> GetProductGalleriesById(long Id)
        {
            return await _context.productGalleries.AsQueryable().
                // Where(c => c.Id == Id).
                FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task DeleteProductGallery(long Id)
        {
            var currentGallery = await _context.productGalleries.AsQueryable().
                // Where(c => c.Id == Id).
                FirstOrDefaultAsync(c => c.Id == Id);

            if (currentGallery != null)
            {
                currentGallery.IsDelete = true;
                _context.productGalleries.Update(currentGallery);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddProductFeatuers(ProductFeature featuer)
        {
            await _context.productFeatures.AddAsync(featuer);
        }

        public async Task<List<ProductFeature>> GetProductFeatures(long productId)
        {
            return await _context.productFeatures.AsQueryable()
                .Where(c => c.ProductId == productId && !c.IsDelete).ToListAsync();

        }
        public async Task DeleteFeatures(long id)
        {
            var currentFeatuer = await _context.productFeatures.AsQueryable().
                Where(c => c.Id == id).FirstOrDefaultAsync();
            if (currentFeatuer != null)
            {
                currentFeatuer.IsDelete = true;

                _context.productFeatures.Update(currentFeatuer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductItemViewModel>> ShowAllProductInSlider()
        {
            var allProduct = await _context.Products
                .Include(c => c.ProductComments)
                .Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory).AsQueryable().
                Select(c => new ProductItemViewModel
                {
                    ProductCategory = c.ProductSelectedCategories.Select(c => c.ProductCategory).First(),
                    CommentCount = c.ProductComments.Count(),
                    Price = c.Price,
                    ProductId = c.Id,
                    ProductImageName = c.ProductImageName,
                    ProductName = c.Name
                }).ToListAsync();

            return allProduct;
        }
        #endregion

        public async Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName)
        {
            //var allProducts = await _context.productCategories.Include(c => c.ProductSelectedCategories).ThenInclude(c => c.Product)
            //    .Where(c => c.UrlName == hrefName)
            //    .Select(c => c.ProductSelectedCategories.Select(c => c.Product))
            //    .ToListAsync();

            var product = await _context.Products
                .Include(c => c.ProductComments)
                .Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .Where(c => c.ProductSelectedCategories.
            Any(c => c.ProductCategory.UrlName == hrefName)).ToListAsync();



            var data = product.Select(c => new ProductItemViewModel
            {
                ProductCategory = c.ProductSelectedCategories.Select(c => c.ProductCategory).First(),
                CommentCount = c.ProductComments.Count(),
                Price = c.Price,
                ProductId = c.Id,
                ProductImageName = c.ProductImageName,
                ProductName = c.Name
            }).ToList();

            return data;
        }

        public async Task<List<ProductItemViewModel>> LastProduct()
        {
            var lastProduct = await _context.Products.
                Include(c => c.ProductSelectedCategories).
                ThenInclude(c => c.ProductCategory).
                AsQueryable().
                OrderByDescending(c => c.CreateDate).
                Select(c => new ProductItemViewModel
                {
                    ProductCategory = c.ProductSelectedCategories.Select(c => c.ProductCategory).First(),
                    CommentCount = 0,
                    Price = c.Price,
                    ProductId = c.Id,
                    ProductImageName = c.ProductImageName,
                    ProductName = c.Name
                }).Take(8).ToListAsync();

            return lastProduct;
        }

        public async Task<ProductDetailsViewModel> ShowProductDetails(long productId)
        {
            return await _context.Products.Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .Include(c => c.ProductGalleries)
                .AsQueryable()
                .Where(c => c.Id == productId)
                .Include(c => c.ProductComments)
               .Select(c => new ProductDetailsViewModel
               {
                   ProductCategory = c.ProductSelectedCategories.
                    Select(c => c.ProductCategory).First(),
                   Description = c.Description,
                   Name = c.Name,
                   Price = c.Price,
                   ProductComment = c.ProductComments.Count(),
                   ProductFeatures = c.ProductFeature.ToList(),
                   ProductId = c.Id,
                   ProductImageName = c.ProductImageName,
                   ShortDescription = c.ShortDescription,
                   ProductImages = c.ProductGalleries.Where(c => !c.IsDelete).Select(x => x.ImageName).ToList()

               }).FirstOrDefaultAsync();
        }

        public async Task AddProductComment(ProductComment comment)
        {
            await _context.ProductComments.AddAsync(comment);
        }

        public async Task<List<ProductComment>> AllProductCommentById(long productId)
        {
            return await _context.ProductComments.Include(c => c.User).AsQueryable().
                Where(c => c.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductItemViewModel>> GetRelatedProduct(string catName, long productId)
        {
            var product = await _context.Products
               .Include(c => c.ProductComments)
               .Include(c => c.ProductSelectedCategories)
               .ThenInclude(c => c.ProductCategory)
               .Where(c => c.ProductSelectedCategories.
           Any(c => c.ProductCategory.UrlName == catName) && c.Id != productId).Take(6).ToListAsync();


            var data = product.Select(c => new ProductItemViewModel
            {
                ProductCategory = c.ProductSelectedCategories.Select(c => c.ProductCategory).First(),
                CommentCount = c.ProductComments.Count(),
                Price = c.Price,
                ProductId = c.Id,
                ProductImageName = c.ProductImageName,
                ProductName = c.Name
            }).ToList();

            return data;
        }
    }
}

