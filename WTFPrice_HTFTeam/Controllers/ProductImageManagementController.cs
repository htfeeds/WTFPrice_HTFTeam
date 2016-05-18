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
    public class ProductImageManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: ProductImageManagement
        public async Task<ActionResult> Index()
        {
            var productImages = db.ProductImages.Include(p => p.Product);
            return View(await productImages.ToListAsync());
        }

        // GET: ProductImageManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = await db.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImageManagement/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ProductImageManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId")] ProductImage productImage, HttpPostedFileBase ImageUrl, bool update)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    productImage.ImageUrl = UploadImage(ImageUrl);
                }

                db.ProductImages.Add(productImage);
                await db.SaveChangesAsync();

                if (update)
                {
                    return PartialView("_ProductImageRecordPartial", productImage);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productImage.ProductId);
            return View(productImage);
        }

        // GET: ProductImageManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = await db.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productImage.ProductId);
            return View(productImage);
        }

        // POST: ProductImageManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId")] ProductImage productImage, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    productImage.ImageUrl = UploadImage(ImageUrl);
                }
                db.Entry(productImage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productImage.ProductId);
            return View(productImage);
        }

        // GET: ProductImageManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = await db.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImageManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductImage productImage = await db.ProductImages.FindAsync(id);
            db.ProductImages.Remove(productImage);
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
