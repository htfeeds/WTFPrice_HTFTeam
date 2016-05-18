using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class ProductsController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: /Products
        [Route("Products")]
        public ActionResult Index()
        {
            int pId = (db.Categories.FirstOrDefault(x => x.Name.Equals("Product"))).Id;
            return RedirectToAction("List", new { category = pId });
        }

        // GET: /Products/List
        [Route("Products/List-{category}")]
        public ActionResult List(int category, decimal? min, decimal? max, int? manufacturer, bool? des)
        {
            var cate = db.Categories.FirstOrDefault(x => x.Id == category);
            if (cate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = cate.Name;
            var mappings = db.Product_Category_Mappings.Where(x => x.CategoryId == category);
            var products = db.Products.Join(mappings, p => p.Id, m => m.ProductId, (p, m) => new { Prod = p });
            if (min.HasValue)
            {
                products.Where(x => x.Prod.Price >= min);
            }
            if (max.HasValue)
            {
                products.Where(x => x.Prod.Price <= max);
            }
            if (manufacturer.HasValue)
            {
                products.Where(x => x.Prod.ManufacturerId == manufacturer);
            }
            var dataModel = new List<Product>();
            foreach (var item in products)
            {
                dataModel.Add(item.Prod);
            }
            if (des.HasValue)
            {
                if (des.Value == true)
                {
                    return View(dataModel.OrderByDescending(x => x.Price));
                }
            }
            return View(dataModel.OrderBy(x => x.Price));
        }

        //GET: /ProductDetails-5
        [Route("ProductDetails-{id}")]
        public ActionResult Details(int id)
        {
            var product = db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //GET: /AddToCart-5
        [Route("AddToCart-{productId}")]
        public ActionResult AddToCart(int productId)
        {
            string s = "";

            if (Request.Cookies["ShoppingCart"] != null && !Request.Cookies["ShoppingCart"].Value.Trim().Equals(""))
            {
                s = Request.Cookies["ShoppingCart"].Value;
                s += " " + productId;
            }
            else
            {
                s += productId;
            }

            Response.Cookies["ShoppingCart"].Value = s;
            Response.Cookies["ShoppingCart"].Expires = DateTime.Now.AddDays(7);

            string[] products = s.Split(' ');
            int no = products.Length;

            return Content("<i class=\"fa fa-shopping-cart\"></i> Giỏ hàng (" + no + ")");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var result = new List<KeyValuePair<string, string>>();
            var products = db.Products.Where(x => x.Name.Contains(term)).Take(5).ToList();
            foreach (var item in products)
            {
                result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.Name));
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: /Products/Search/apple
        [Route("Products/Search/{query}")]
        public ActionResult Search(string query)
        {           
            var model = db.Products.Where(x => x.Name.Contains(query)).ToList();
            return View(model);
        }
    }
}