using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;
using System.IO;

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: ProductManagement
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Manufacturer);
            return View(await products.ToListAsync());
        }

        // GET: ProductManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductManagement/Create
        public ActionResult Create()
        {
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name");
            return View();
        }

        // POST: ProductManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Price,Stock,Info,Warranty,Promotion,ManufacturerId,ShowOnHomePage,GalleryActived")] Product product, HttpPostedFileBase ImageUrl, HttpPostedFileBase GalleryImageUrl, string subject)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    product.ImageUrl = UploadImage(ImageUrl);
                }
                if (GalleryImageUrl != null && GalleryImageUrl.ContentLength > 0)
                {
                    product.GalleryImageUrl = UploadImage(GalleryImageUrl);
                }

                product.CreatedOn = DateTime.Now;
                db.Products.Add(product);
                await db.SaveChangesAsync();

                if (subject.Equals("1"))
                {
                    int pid = product.Id;
                    return RedirectToAction("Edit", new { id = pid });
                }

                return RedirectToAction("Index");
            }

            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            return View(product);
        }

        // GET: ProductManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name");
            return View(product);
        }

        // POST: ProductManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Price,Stock,Info,Warranty,Promotion,ManufacturerId,ShowOnHomePage,GalleryActived,CreatedOn")] Product product, HttpPostedFileBase ImageUrl, HttpPostedFileBase GalleryImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    product.ImageUrl = UploadImage(ImageUrl);
                }
                else
                {
                    var oldProduct = db.Products.FirstOrDefault(x => x.Id == product.Id);
                    if (oldProduct.ImageUrl != null)
                    {
                        product.ImageUrl = oldProduct.ImageUrl;
                    }
                }
                if (GalleryImageUrl != null && GalleryImageUrl.ContentLength > 0)
                {
                    product.GalleryImageUrl = UploadImage(GalleryImageUrl);
                }
                else
                {
                    var oldProduct = db.Products.FirstOrDefault(x => x.Id == product.Id);
                    if (oldProduct.GalleryImageUrl != null)
                    {
                        product.GalleryImageUrl = oldProduct.GalleryImageUrl;
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name");
            return View(product);
        }

        // GET: ProductManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //HTF-Added
        private string UploadImage(HttpPostedFileBase file)
        {
            const string defaultFolderToSaveFile = "~/Content/images/Products/";
            if (Directory.Exists(Server.MapPath(defaultFolderToSaveFile)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(defaultFolderToSaveFile));
            }

            var fileName = Path.GetFileName(file.FileName);

            var path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName);
            var i = 1;
            while (System.IO.File.Exists(path))
            {
                path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName + "_" + i);
                i++;
            }

            file.SaveAs(path);

            var imageUrl = defaultFolderToSaveFile + fileName;
            return imageUrl;
        }
    }
}
