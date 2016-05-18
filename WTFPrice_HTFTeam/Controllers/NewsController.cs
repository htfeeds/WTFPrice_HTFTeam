using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WTFPrice_HTFTeam.Models;
using WTFPrice_HTFTeam.ViewModel;

namespace WTFPrice_HTFTeam.Controllers
{

    public class NewsController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();
        private NewsVM nVM = new NewsVM();
        private Repository nRp=new Repository();
        // GET: News
        public ActionResult Index(int? id)
        {
            
            nRp.SortByCategoryTech = db.News.Where(x => x.CategoryId == 12).OrderByDescending(x=>x.CreatedOn).Take(5).ToList();
            nRp.SortByCategoryEvaluate = db.News.Where(x => x.CategoryId == 13).OrderByDescending(x=>x.CreatedOn).Take(5).ToList();
            nRp.SortByCategoryDiscount = db.News.Where(x => x.CategoryId == 17).OrderByDescending(x=>x.CreatedOn).Take(5).ToList();

            if (id.HasValue)
            {
                if (id.Value == 12)
                {
                    nVM.SortByCreatedDate = db.News.Where(x => x.CategoryId == id.Value).OrderByDescending(x => x.CreatedOn).Take(6).ToList();
                    Session["CategoryName"] = "Technology";
                }
                else if (id.Value == 13)
                {
                    nVM.SortByCreatedDate = db.News.Where(x => x.CategoryId == id.Value).OrderByDescending(x => x.CreatedOn).Take(6).ToList();
                    Session["CategoryName"] = "Evaluate";
                }
                else if (id.Value==17)
                {
                    nVM.SortByCreatedDate = db.News.Where(x => x.CategoryId == id.Value).OrderByDescending(x => x.CreatedOn).Take(6).ToList();
                    Session["CategoryName"] = "Discount";
                }
                else
                {
                    return PartialView("PartialNews/_PartialNotFound");
                }
            }
            else
            {
                nVM.SortByCreatedDate = db.News.OrderByDescending(x => x.CreatedOn).Take(6).ToList();
                Session["CategoryName"] = "Index";
            }
            nVM.SortByViews = db.News.OrderByDescending(x => x.ViewCount).Take(10).ToList();
            nVM.SortByCategory = nRp;
            return View(nVM);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            nRp.SortByCategoryTech = db.News.Where(x => x.CategoryId == 12).OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            nRp.SortByCategoryEvaluate = db.News.Where(x => x.CategoryId == 13).OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            nRp.SortByCategoryDiscount = db.News.Where(x => x.CategoryId == 17).OrderByDescending(x => x.CreatedOn).Take(5).ToList();


            nVM.SortByCreatedDate = db.News.OrderByDescending(x => x.CreatedOn).ToList();
            nVM.SortByViews = db.News.OrderByDescending(x => x.ViewCount).Take(10).ToList();
            nVM.SortByCategory = nRp;
            News news = db.News.Find(id);
            if (news == null)
            {
                return PartialView("PartialNews/_PartialNotFound");
            }
            news.ViewCount++;
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.news = news;
            return View(nVM);
        }

        public ActionResult ViewMore(int times)
        {
           Thread.Sleep(800);
            string str = Session["CategoryName"] as string;
            List<News> newsPage;
            if (str == "Technology")
            {
                newsPage = db.News.Where(x => x.CategoryId == 12).OrderByDescending(x => x.CreatedOn).Skip(times * 6).Take(6).ToList();
            }
            else if (str=="Evaluate")
            {
                newsPage = db.News.Where(x => x.CategoryId == 13).OrderByDescending(x => x.CreatedOn).Skip(times * 6).Take(6).ToList();
            }
            else if (str == "Discount")
            {
                newsPage = db.News.Where(x => x.CategoryId == 17).OrderByDescending(x => x.CreatedOn).Skip(times * 6).Take(6).ToList();
            }
            else if (str == "Index")
            {
                newsPage = db.News.OrderByDescending(x => x.CreatedOn).Skip(times * 6).Take(6).ToList();
            }
            else
            {
                return new HttpNotFoundResult();
            }
            Response.Cookies["Hidden"].Value = newsPage.Count.ToString();
            Response.Cookies["Hidden"].Expires = DateTime.Now.AddDays(2);
            //List<News> newsPage = db.News.OrderByDescending(x => x.CreatedDate).Skip((count++)*2).Take(2).ToList();
           
            return PartialView("PartialNews/_PartialNewsLeft",newsPage);
        }
        [HttpPost]
        public ActionResult Search(string searchInput)
        {
            Thread.Sleep(500);
            var news = db.News.AsQueryable();
            if (string.IsNullOrEmpty(searchInput) == false)
            {
                news = news.Where(x => x.Title.Contains(searchInput));
            }
            ViewBag.Name = "Search";
            ViewBag.CountSearch = news.Count();
            return PartialView("PartialNews/_PartialNewsLeft", news.ToList());
        }

        public ActionResult QuickSearch(string term)
        {
            var title = db.News.Where(x=>x.Title.Contains(term)).ToList().Select(x => new { value = x.Title});

            return Json(title, JsonRequestBehavior.AllowGet);
        }

    }
}