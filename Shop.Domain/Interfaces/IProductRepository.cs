using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task SaveChange();

        #region categories
        Task<bool> CheckUrlNameCategory(string urlName);
        Task<bool> CheckUrlNameCategory(string urlName, long categoryId);
        Task AddProductCategory(ProductCategory productCategory);
        Task<ProductCategory> GetProductCategoryById(long id);
        Task<List<ProductCategory>> GetAllProductCategories();
        Task<List<long>> GetAllProductCategoriesId(long productId);
        void UpdateProduct(Product product);
        void UpdateProductCategory(ProductCategory Category);

        Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter);
        #endregion

        #region product
        Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter);
        Task AddProduct(Product product);
        Task RemoveProductSelectedCategories(long productId);
        Task<Product>GetProductById(long productId);
        Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);
        Task AddProductGalleries(List<ProductGalleries> productGalleries);
        Task<bool> CheckProduct(long productId);
        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task<ProductGalleries> GetProductGalleriesById(long Id);
        Task DeleteProductGallery(long Id);
        Task AddProductFeatuers(ProductFeature featuer);
        Task<List<ProductFeature>> GetProductFeatures(long productId);
        Task DeleteFeatures(long id);
        Task<List<ProductItemViewModel>> ShowAllProductInSlider();
        Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName);
        Task<List<ProductItemViewModel>> LastProduct();

        Task<ProductDetailsViewModel> ShowProductDetails(long productId);
        Task AddProductComment(ProductComment comment);
        Task<List<ProductComment>> AllProductCommentById(long productId);
        Task<List<ProductItemViewModel>> GetRelatedProduct(string catName, long productId);


        #endregion

    }
}
