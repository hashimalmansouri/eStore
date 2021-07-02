using eStore.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace eStore.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? brandId, int? genreId, string search, int? pageNo)
        {
            var products = from p in db.Products.Include(b => b.Attachments) select p;
            if (brandId != null)
            {
                products = products.Where(b => b.BrandId == brandId);
                ViewBag.BrandId = brandId;
                ViewBag.Name = db.Brands.SingleOrDefault(x => x.BrandId == brandId).BrandName;
            }
            if (genreId != null)
            {
                products = products.Where(g => g.GenreId == genreId);
                ViewBag.GenreId = genreId;
                ViewBag.Name = db.Genres.SingleOrDefault(x => x.GenreId == genreId).GenreName;
            }
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            products = products.OrderByDescending(p => p.ProductId);
            return View(products.ToPagedList(pageNo ?? 1, 6));
        }
        public ActionResult ProductDetails(int? id)
        {
            var product = db.Products.Include(x => x.Attachments).Include(x => x.Brand).Include(x => x.Attachments).FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }
        [ChildActionOnly]
        public ActionResult BrandsMenu()
        {
            return PartialView(db.Brands.ToList());
        }
        [ChildActionOnly]
        public ActionResult GenresMenu()
        {
            return PartialView(db.Genres.ToList());
        }
    }
}