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
    public class MappingManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: MappingManagement
        public async Task<ActionResult> Index()
        {
            var product_Category_Mappings = db.Product_Category_Mappings.Include(p => p.Category).Include(p => p.Product);
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            return View(await product_Category_Mappings.ToListAsync());
        }

        // GET: MappingManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Category_Mappings product_Category_Mappings = await db.Product_Category_Mappings.FindAsync(id);
            if (product_Category_Mappings == null)
            {
                return HttpNotFound();
            }
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            return View(product_Category_Mappings);
        }

        // GET: MappingManagement/Create
        public ActionResult Create()
        {
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: MappingManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId,CategoryId")] Product_Category_Mappings product_Category_Mappings, bool update)
        {
            if (ModelState.IsValid)
            {
                db.Product_Category_Mappings.Add(product_Category_Mappings);
                await db.SaveChangesAsync();

                if (update)
                {
                    List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
                    categories.Insert(0, new ParentCategory(0));

                    ViewBag.ParentCategory = categories;
                    return PartialView("_ProductMappingRecordPartial", product_Category_Mappings);
                }
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product_Category_Mappings.CategoryId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", product_Category_Mappings.ProductId);
            return View(product_Category_Mappings);
        }

        // GET: MappingManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Category_Mappings product_Category_Mappings = await db.Product_Category_Mappings.FindAsync(id);
            if (product_Category_Mappings == null)
            {
                return HttpNotFound();
            }
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product_Category_Mappings.CategoryId);

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", product_Category_Mappings.ProductId);
            return View(product_Category_Mappings);
        }

        // POST: MappingManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId,CategoryId")] Product_Category_Mappings product_Category_Mappings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Category_Mappings).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product_Category_Mappings.CategoryId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", product_Category_Mappings.ProductId);
            return View(product_Category_Mappings);
        }

        // GET: MappingManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Category_Mappings product_Category_Mappings = await db.Product_Category_Mappings.FindAsync(id);
            if (product_Category_Mappings == null)
            {
                return HttpNotFound();
            }
            return View(product_Category_Mappings);
        }

        // POST: MappingManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product_Category_Mappings product_Category_Mappings = await db.Product_Category_Mappings.FindAsync(id);
            db.Product_Category_Mappings.Remove(product_Category_Mappings);
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
