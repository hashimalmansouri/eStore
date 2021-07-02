using eStore.Models;
using eStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eStore.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the Product from the database
            var addedProduct = db.Products
                .Single(p => p.ProductId == id);


            if (addedProduct.Quantity > 0 && addedProduct.TempQuantity > 0 && (addedProduct.TempQuantity <= addedProduct.Quantity))
            {
                // Add it to the shopping cart
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.AddToCart(addedProduct);
                //////////////////////////////////
                addedProduct.TempQuantity -= 1;
                db.Entry(addedProduct).State = EntityState.Modified;
                db.SaveChanges();
                //////////////////////////////////
            }
            else
            {
                return RedirectToAction("QuantityOut");
            }

            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "Home");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the product to display confirmation
            string productName = db.Carts
                .Single(item => item.RecordId == id).Product.ProductName;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "تم حذف المنتج " + Server.HtmlEncode(productName) +
                          " من سلة التسوق.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            // increase quantity
            return Json(results);
        }
        [ChildActionOnly]
        public ActionResult QuantityOut()
        {
            return View();
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}