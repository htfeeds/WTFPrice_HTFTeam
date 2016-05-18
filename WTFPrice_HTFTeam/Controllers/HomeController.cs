using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class HomeController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();
        public ActionResult Index()
        {
            ViewBag.HomeGallery = db.Products.Where(x => x.GalleryActived == true).ToList();
            ViewBag.HomeNews = db.News.Where(x => x.ShowOnHomePage == true).ToList();
            var dataModel = db.Products.Where(x => x.ShowOnHomePage == true).OrderByDescending(x=>x.Id).Take(24);
            return View(dataModel.ToList());
        }
    }
}