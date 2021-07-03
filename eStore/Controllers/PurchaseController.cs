using eStore.Models;
using eStore.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eStore.Controllers
{
    [Authorize(Roles = "مستهلك")]
    public class PurchaseController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Purchase
        public ActionResult Index(int? pageNo)
        {
            ViewBag.Balance = db.Users.Find(User.Identity.GetUserId()).Balance;
            var purchases = db.OrderDetails.Where(x => x.Order.Username == User.Identity.Name)
                .Select(x => new PurchaseViewModel() 
                {
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    OrderDate = x.Order.OrderDate,
                    ProductName = x.Product.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                });
            return View(purchases.OrderByDescending(x => x.OrderDate).ToPagedList(pageNo ?? 1, 20));
        }

        public ActionResult Replace(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.OrderDetails.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var currentProduct = db.Products.Find(id);
            if (currentProduct == null)
            {
                return HttpNotFound();
            }
            if (currentProduct.Quantity < product.Quantity)
            {
                TempData["Message"] = "عذراً، لاتوجد كمية كافية في المتجر، سنقوم بإعلامك عندما تتوفر.";
                return RedirectToAction("Index");
            }
            else
            {
                currentProduct.Quantity += product.Quantity;
                db.OrderDetails.Remove(product);
                db.Entry(currentProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddToCart", new { id = currentProduct.ProductId, x = product.Quantity });
            }
        }

        public ActionResult Retrun(int? id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.OrderDetails.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var price = product.Quantity * product.UnitPrice;
            user.Balance += price - (price * (5 / 100));
            var currentProduct = db.Products.Find(id);
            if (currentProduct == null)
            {
                return HttpNotFound();
            }
            var order = db.Orders.Find(product.OrderId);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Total -= price - (price * (5 / 100));
            currentProduct.Quantity += product.Quantity;
            db.Entry(user).State = EntityState.Modified;
            db.Entry(order).State = EntityState.Modified;
            db.Entry(currentProduct).State = EntityState.Modified;
            db.OrderDetails.Remove(product);
            db.SaveChanges();
            TempData["Message"] = "تم إرجاع المنتج " + currentProduct.ProductName + " مع خصم 5% من القيمة.";
            return RedirectToAction("Index");
        }


        public ActionResult AddToCart(int id, int x)
        {
            // Retrieve the Product from the database
            var addedProduct = db.Products
                .Single(p => p.ProductId == id);
            for (int i = 0; i < x; i++)
            {
                var cart = ShoppingCart.GetCart(HttpContext);
                cart.AddToCart(addedProduct);
                addedProduct.TempQuantity -= 1;
                db.Entry(addedProduct).State = EntityState.Modified;
            }
            db.SaveChanges();
            TempData["Message"] = "تمت إضافة المنتج " + addedProduct.ProductName + " إلى السلّة.";
            return RedirectToAction("Index");
        }
    }
}