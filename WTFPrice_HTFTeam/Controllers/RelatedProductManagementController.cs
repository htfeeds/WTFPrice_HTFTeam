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

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RelatedProductManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: RelatedProductManagement
        public async Task<ActionResult> Index()
        {
            var relatedProducts = db.RelatedProducts.Include(r => r.Product);
            ViewBag.SimilarProduct = db.Products.ToList();
            return View(await relatedProducts.ToListAsync());
        }

        // GET: RelatedProductManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelatedProduct relatedProduct = await db.RelatedProducts.FindAsync(id);
            if (relatedProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.SimilarProduct = db.Products.ToList();
            return View(relatedProduct);
        }

        // GET: RelatedProductManagement/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: RelatedProductManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId,SimilarProductId")] RelatedProduct relatedProduct, bool update)
        {
            if (ModelState.IsValid)
            {
                db.RelatedProducts.Add(relatedProduct);
                await db.SaveChangesAsync();

                if (update)
                {
                    return PartialView("_ProductRelatedRecordPartial", relatedProduct);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", relatedProduct.ProductId);
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name");
            return View(relatedProduct);
        }

        // GET: RelatedProductManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelatedProduct relatedProduct = await db.RelatedProducts.FindAsync(id);
            if (relatedProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", relatedProduct.ProductId);
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name", relatedProduct.SimilarProductId);
            return View(relatedProduct);
        }

        // POST: RelatedProductManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId,SimilarProductId")] RelatedProduct relatedProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(relatedProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", relatedProduct.ProductId);
            ViewBag.SimilarProductId = new SelectList(db.Products, "Id", "Name", relatedProduct.SimilarProductId);
            return View(relatedProduct);
        }

        // GET: RelatedProductManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelatedProduct relatedProduct = await db.RelatedProducts.FindAsync(id);
            if (relatedProduct == null)
            {
                return HttpNotFound();
            }
            return View(relatedProduct);
        }

        // POST: RelatedProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RelatedProduct relatedProduct = await db.RelatedProducts.FindAsync(id);
            db.RelatedProducts.Remove(relatedProduct);
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
    }
}
