using eStore.Models;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;

namespace eStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.Total = cart.GetOrderTotal(order);
                //Save Order
                db.Orders.Add(order);
                db.SaveChanges();
                //Process the order
                cart.CreateOrder(order);
                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                     o.Username == User.Identity.Name);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}