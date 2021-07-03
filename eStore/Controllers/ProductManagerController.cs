using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using eStore.Models;

namespace eStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductManager
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
            return View(products.ToPagedList(pageNo ?? 1, 5));
        }

        // GET: ProductManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(p => p.Attachments).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductManager/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: ProductManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Description,Quantity,TempQuantity,BrandId,GenreId")] Product product, List<HttpPostedFileBase> upload)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Any(n => n.ProductName == product.ProductName))
                {
                    ViewBag.Create = "هذا المنتج موجود.";
                    ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
                    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
                    return View(product);
                }
                if (upload != null)
                {
                    var attachments = new List<Attachment>();
                    foreach (HttpPostedFileBase item in upload)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), item.FileName);
                        item.SaveAs(path);
                        var obj = new Attachment() { FileName = item.FileName };
                        attachments.Add(obj);
                    }
                    product.Attachments = attachments;
                }
                product.TempQuantity = product.Quantity;
                product.DateCreated = DateTime.Today.Date;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // GET: ProductManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // POST: ProductManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Description,Quantity,TempQuantity,BrandId,GenreId")] Product product, List<HttpPostedFileBase> upload)
        {
            if (ModelState.IsValid)
            {
                if (upload.FirstOrDefault() != null)
                {
                    var attachments = new List<Attachment>();
                    foreach (HttpPostedFileBase item in upload)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), item.FileName);
                        item.SaveAs(path);
                        var obj = new Attachment() { FileName = item.FileName, ProductId = product.ProductId };
                        attachments.Add(obj);
                    }
                    db.Attachments.AddRange(attachments);
                }
                product.TempQuantity = product.Quantity;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // GET: ProductManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(p => p.Attachments).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteImage(int? id, int? imgId)
        {
            if (id == null || imgId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var image = db.Attachments.FirstOrDefault(x => x.ProductId == id && x.Id == imgId);
            if (image == null)
            {
                return HttpNotFound();
            }
            string oldPath = Path.Combine(Server.MapPath("~/Content/Images/Products"), image.FileName);
            if (oldPath != null)
            {
                System.IO.File.Delete(oldPath);
                db.Attachments.Remove(image);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
