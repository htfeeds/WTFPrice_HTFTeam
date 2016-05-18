using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class CompareController : Controller
    {
        WTFPriceEntities db = new WTFPriceEntities();
        // GET: Compare
        public ActionResult Compare(int productId, int? similarId)
        {
            if (similarId.HasValue)
            {
                Product similarProduct = db.Products.FirstOrDefault(n => n.Id == similarId.Value);
                ViewBag.sProduct = similarProduct;
            }

            Product product = db.Products.FirstOrDefault(x => x.Id == productId);
            return View(product);
        }
    }

}