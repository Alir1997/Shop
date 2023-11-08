using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Web.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace Shop.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constractor
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService= productService;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            ViewData["LastProducts"] = await _productService.LastProduct();
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        


    }
}