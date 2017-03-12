using SportsStore.WebUI.Models;
using SportStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository respository;
        public int PageSize = 1;
        public ProductController(IProductRepository productRepository)
        {
            this.respository = productRepository;
        }
        public ViewResult List(int page=1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = respository.Products.OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPage = PageSize,
                   TotalItems=respository.Products.Count()
                }
            };
            return View(model);
        }
    }
}