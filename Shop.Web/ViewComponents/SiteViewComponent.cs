using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Sliders;
using Shop.Web.Extentions;
using System.Threading.Tasks;

namespace Shop.Web.ViewComponets
{
    #region site header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public SiteHeaderViewComponent(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userService.GetUserByPhoneNumber(User.Identity.Name);
                var order = await _orderService.GetUserBasket(User.GetUserId());
                ViewBag.Order = await _orderService.GetUserBasket(User.GetUserId());
                ViewBag.UserCompare = await _userService.GetUserCompares(User.GetUserId());
                ViewBag.FavoriteCount = await _userService.UserFavoriteCount(User.GetUserId());
            }
            return View("SiteHeader");
        }
    }
    #endregion

    #region site footer
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("SiteFooter");
        }
    }
    #endregion

    #region slider-home
    public class SliderHomeViewComponent : ViewComponent
    {
        #region constractor
        private readonly ISiteSettingService _siteSettingService;
        public SliderHomeViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FilterSlidersViewModel = new FilterSlidersViewModel()
            {
                TakeEntity = 10
            };
            var data = await _siteSettingService.FilterSliders(FilterSlidersViewModel);
            return View("SliderHome", data);
        }
    }
    #endregion

    #region popular category-home
    public class PopularCategoryViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public PopularCategoryViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FilterCategory = new FilterProductCategoriesViewModel()
            {
                TakeEntity = 6
            };
            var data = await _productService.FilterProductCategories(FilterCategory);
            return View("PopularCategory", data);
        }
    }
    #endregion

    #region popular category-home
    public class SideBarCategoryViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public SideBarCategoryViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FilterCategory = new FilterProductCategoriesViewModel();

            var data = await _productService.FilterProductCategories(FilterCategory);
            return View("SideBarCategory", data);
        }
    }
    #endregion

    #region All Product In Slider
    public class AllProductInSliderViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllProductInSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInSlider();
            return View("AllProductInSlider", data);
        }
    }
    #endregion

    #region All Product In categoryPc
    public class AllInCategoryPcViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllInCategoryPcViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("pc");
            return View("AllInCategoryPc", data);
        }
    }
    #endregion

    #region All Product In categoryTv
    public class AllInCategoryTvViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllInCategoryTvViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("tv");
            return View("AllInCategoryTv", data);
        }
    }
    #endregion

    #region All Product In category Mobile
    public class AllInCategoryMobileViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllInCategoryMobileViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("mobile");
            return View("AllInCategoryMobile", data);
        }
    }
    #endregion

    #region All Product In category watch
    public class AllInCategoryWatchViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllInCategoryWatchViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("watch");
            return View("AllInCategoryWatch", data);
        }
    }
    #endregion

    #region All Product In category Janebi
    public class AllInCategoryXioViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public AllInCategoryXioViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("xio");
            return View("AllInCategoryXio", data);
        }
    }
    #endregion

    #region All Product In category Janebi
    public class ProductCommentsViewComponent : ViewComponent
    {
        #region constractor
        private readonly IProductService _productService;
        public ProductCommentsViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(long productId)
        {
            var data = await _productService.AllProductCommentById(productId);
            return View("ProductComments", data);
        }
    }
    #endregion
}
