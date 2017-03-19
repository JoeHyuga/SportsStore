using SportsStore.WebUI.Models;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repo,IOrderProcessor proc)
        {
            orderProcessor = proc;
            repository = repo;
        }
        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel {
                Cart=cart,ReturnUrl=returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product!= null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index",new { returnUrl});
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index",new { returnUrl});
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public PartialViewResult Checkout()
        {
            return PartialView(new ShoppingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShoppingDetails shoppingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("","Sorry,your cart isempty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shoppingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shoppingDetails);
            }
        }
    }
}