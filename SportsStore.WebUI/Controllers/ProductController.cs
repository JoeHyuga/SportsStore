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
        public ProductController(IProductRepository productRepository)
        {
            this.respository = productRepository;
        }
        public ViewResult List()
        {
            return View(respository.Products);
        }
    }
}