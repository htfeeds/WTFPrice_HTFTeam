using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: NewsManagement
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.NewsCategory).OrderByDescending(x => x.CreatedOn);
            ViewBag.Count = news.Count();
            return View(news.ToList());
        }

        // GET: NewsManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            return View(news);
        }

        // GET: NewsManagement/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name");
            return View();
        }

        // POST: NewsManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Short,FullText,Author,HomeImageUrl,ShowOnHomePage,Published,CategoryId,CreatedDate,Views")] News news)
        {
            if (ModelState.IsValid)
            {
                news.CreatedOn = DateTime.Now;
                news.ViewCount = 0;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: NewsManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: NewsManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Short,FullText,Author,,HomeImageUrl,ShowOnHomePage,Published,CategoryId,CreatedDate,Views")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: NewsManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            return View(news);
        }

        // POST: NewsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
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
