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
    public class CategoryManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: CategoryManagement
        public async Task<ActionResult> Index()
        {
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            return View(await db.Categories.ToListAsync());
        }

        // GET: CategoryManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategory = categories;
            return View(category);
        }

        // GET: CategoryManagement/Create
        public ActionResult Create()
        {
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: CategoryManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", category.Id);
            return View(category);
        }

        // GET: CategoryManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            List<ParentCategory> categories = db.Categories.ToList().Select(x => new ParentCategory(x.Id)).ToList();
            categories.Insert(0, new ParentCategory(0));

            ViewBag.ParentCategoryId = new SelectList(categories, "Id", "Name", category.ParentCategoryId);
            return View(category);
        }

        // POST: CategoryManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: CategoryManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: CategoryManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
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
