using eStore.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string search, int? pageNo)
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Genre).Include(p => p.Attachments);
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            products = products.OrderBy(p => p.ProductId);
            return View(products.ToPagedList(pageNo ?? 1, 6));
        }

        public ActionResult Contact(int? id)
        {
            if (id != null)
                ViewBag.Message = "تم الإرسال بنجاح.";
            return View();
        }
    }
}