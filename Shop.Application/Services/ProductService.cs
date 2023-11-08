using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class ProductService : IProductService
    {
        #region constractor
        private readonly IProductRepository _ProductRepository;
        private readonly IUserRepository _userRepository;
        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _ProductRepository = productRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region product-admin
        #region product categories
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createCategory, IFormFile image)
        {
            if (await _ProductRepository.CheckUrlNameCategory(createCategory.UrlName))
                return CreateProductCategoryResult.IsExistUrlName;
            var newCategory = new ProductCategory
            {
                UrlName = createCategory.UrlName,
                Title = createCategory.Title,
                ParentId = null,
                IsDelete = false
            };
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtentions.CategoryOrginServer, 150, 150, PathExtentions.CategoryThumbServer);

                newCategory.ImageName = imageName;
            }

            await _ProductRepository.AddProductCategory(newCategory);
            await _ProductRepository.SaveChange();


            return CreateProductCategoryResult.Success;
        }

        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image)
        {
            var productCategory = await _ProductRepository.GetProductCategoryById(editProductCategory.ProductCategoryId);
            if (productCategory == null) return EditProductCategoryResult.NotFound;

            if (await _ProductRepository.CheckUrlNameCategory(editProductCategory.UrlName, editProductCategory.ProductCategoryId))
                return EditProductCategoryResult.IsExistUrlName;
            productCategory.UrlName = editProductCategory.UrlName;
            productCategory.Title = editProductCategory.Title;

            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtentions.CategoryOrginServer, 150, 150, PathExtentions.CategoryThumbServer, productCategory.ImageName);

                productCategory.ImageName = imageName;
            }
            _ProductRepository.UpdateProductCategory(productCategory);
            await _ProductRepository.SaveChange();

            return EditProductCategoryResult.Success;


        }

        public async Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            return await _ProductRepository.FilterProductCategories(filter);
        }



        public async Task<EditProductCategoryViewModel> GetEditProductCategory(long categoryId)
        {
            var productCategory = await _ProductRepository.GetProductCategoryById(categoryId);
            if (productCategory != null)
            {
                return new EditProductCategoryViewModel
                {
                    ImageName = productCategory.ImageName,
                    ProductCategoryId = productCategory.Id,
                    Title = productCategory.Title,
                    UrlName = productCategory.UrlName
                };
            }
            return null;

        }
        #endregion

        #region product
        public async Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter)
        {
            return await _ProductRepository.FilterProducts(filter);
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct, IFormFile imageProcudt)
        {
            #region product
            var newProduct = new Product
            {
                Name = createProduct.Name,
                Price = createProduct.Price,
                Description = createProduct.Description,
                ShortDescription = createProduct.ShortDescription,
                IsActive = createProduct.IsActive
            };

            if (imageProcudt != null && imageProcudt.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageProcudt.FileName);

                imageProcudt.AddImageToServer(imageName, PathExtentions.ProductOrginServer, 215, 215, PathExtentions.ProductThumbServer);

                newProduct.ProductImageName = imageName;
            }
            else
            {
                return CreateProductResult.NotImage;
            }
            await _ProductRepository.AddProduct(newProduct);
            await _ProductRepository.SaveChange();
            await _ProductRepository.AddProductSelectedCategories(createProduct.ProductSelectedCategory, newProduct.Id);

            return CreateProductResult.success;
            #endregion
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _ProductRepository.GetAllProductCategories();
        }

        public async Task<EditProductViewModel> GetEditProduct(long productId)
        {
            var currentProdut = await _ProductRepository.GetProductById(productId);

            if (currentProdut != null)
            {
                return new EditProductViewModel
                {
                    Description = currentProdut.Description,
                    IsActive = currentProdut.IsActive,
                    Name = currentProdut.Name,
                    Price = currentProdut.Price,
                    ShortDescription = currentProdut.ShortDescription,
                    ProductImageName = currentProdut.ProductImageName,
                    ProductSelectedCategory = await _ProductRepository.GetAllProductCategoriesId(productId)
                };
            }
            return null;

        }

        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct)
        {
            var product = await _ProductRepository.GetProductById(editProduct.ProductId);
            if (product == null) return EditProductResult.NotFound;
            if (editProduct.ProductSelectedCategory == null) return EditProductResult.NotProductSelectedCategoryHasNull;

            #region edit product
            product.ShortDescription = editProduct.ShortDescription;
            product.Description = editProduct.Description;
            product.IsActive = editProduct.IsActive;
            product.Price = editProduct.Price;
            product.Name = editProduct.Name;



            if (editProduct.ProductImage != null && editProduct.ProductImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(editProduct.ProductImage.FileName);
                editProduct.ProductImage.AddImageToServer(imageName, PathExtentions.ProductOrginServer, 255, 273, PathExtentions.ProductThumbServer, product.ProductImageName);

                product.ProductImageName = imageName;
            }
            #endregion
            _ProductRepository.UpdateProduct(product);

            await _ProductRepository.RemoveProductSelectedCategories(editProduct.ProductId);
            await _ProductRepository.AddProductSelectedCategories(editProduct.ProductSelectedCategory, editProduct.ProductId);
            await _ProductRepository.SaveChange();

            return EditProductResult.Success;
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            return await _ProductRepository.DeleteProduct(productId);
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            return await _ProductRepository.RecoverProduct(productId);
        }

        public async Task<bool> AddProductGallery(long productId, List<IFormFile> images)
        {
            if (!await _ProductRepository.CheckProduct(productId))
            {
                return false;
            }
            if (images != null && images.Any())
            {
                var productGallery = new List<ProductGalleries>();
                foreach (var image in images)
                {
                    if (image.IsImage())
                    {
                        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                        image.AddImageToServer(imageName, PathExtentions.ProductOrginServer, 215, 215, PathExtentions.ProductThumbServer);


                        productGallery.Add(new ProductGalleries
                        {
                            ImageName = imageName,
                            ProductId = productId
                        });
                    }
                }
                await _ProductRepository.AddProductGalleries(productGallery);
            }
            return true;
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _ProductRepository.GetAllProductGalleries(productId);
        }

        public async Task DeleteImage(long galleryId)
        {
            var productGallery = await _ProductRepository.GetProductGalleriesById(galleryId);

            if (productGallery != null)
            {
                UploadImageExtension.DeleteImage(productGallery.ImageName, PathExtentions.ProductOrginServer, PathExtentions.ProductThumbServer);
                await _ProductRepository.DeleteProductGallery(galleryId);
            }
        }

        public async Task<CreateProductFeatuersResult> CreateProductFeatuers(CreateProductFeatuersViewModel createProductFeatuers)
        {

            if (!await _ProductRepository.CheckProduct(createProductFeatuers.ProductId))
            {
                return CreateProductFeatuersResult.Error;
            }
            var newProductFeatuers = new ProductFeature
            {
                FeatureTitle = createProductFeatuers.Title,
                FeatureValue = createProductFeatuers.Value,
                ProductId = createProductFeatuers.ProductId
            };

            await _ProductRepository.AddProductFeatuers(newProductFeatuers);
            await _ProductRepository.SaveChange();

            return CreateProductFeatuersResult.Success;
        }

        public async Task<List<ProductFeature>> GetProductFeatures(long productId)
        {
            return await _ProductRepository.GetProductFeatures(productId);
        }

        public async Task DeleteFeatures(long id)
        {
            await _ProductRepository.DeleteFeatures(id);
        }

        public async Task<List<ProductItemViewModel>> ShowAllProductInSlider()
        {
            return await _ProductRepository.ShowAllProductInSlider();
        }

        public async Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName)
        {
            return await _ProductRepository.ShowAllProductInCategory(hrefName);
        }

        public async Task<List<ProductItemViewModel>> LastProduct()
        {
            return await _ProductRepository.LastProduct();
        }

        public async Task<ProductDetailsViewModel> ShowProductDetails(long productId)
        {
            return await _ProductRepository.ShowProductDetails(productId);
        }

        public async Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createProduct, long userId)
        {
            var user=await _userRepository.GetUserById(userId);
            if(user == null)
            {
                return CreateProductCommentResult.CheckUser;
            }
            if(! await _ProductRepository.CheckProduct(createProduct.ProductId))
            {
                return CreateProductCommentResult.CheckUser;
            }
            var newComment = new ProductComment
            {
                ProductId = createProduct.ProductId,
                UserId = userId,
                Text = createProduct.Text

            };
            await _ProductRepository.AddProductComment(newComment);
            await _ProductRepository.SaveChange();
            return CreateProductCommentResult.Success;
        }

        public async Task<List<ProductComment>> AllProductCommentById(long productId)
        {
            return await _ProductRepository.AllProductCommentById(productId);
        }

        public async Task<List<ProductItemViewModel>> GetRelatedProduct(string catName, long productId)
        {
            return await _ProductRepository.GetRelatedProduct(catName,productId);
        }
        #endregion
        #endregion
    }
}
