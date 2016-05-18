using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class OrderController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: Order
        public ActionResult CheckOut()
        {
            var model = getListItems();
            ViewBag.ShoppingCart = model;
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            var model = getListItems();
            if (User.Identity.IsAuthenticated)
            {
                order.UserId = User.Identity.GetUserId();
            }
            else
            {
                order.UserId = "Unknown";
            }
            order.CreatedOn = DateTime.Now;
            order.Status = false;

            if (model != null)
            {
                order.OrderDetails = new List<OrderDetail>();
                foreach (ItemCart item in model)
                {
                    var od = new OrderDetail();
                    od.ProductId = item.ProductId;
                    od.Quantity = item.Quantity;

                    order.OrderDetails.Add(od);
                }
            }

            db.Orders.Add(order);
            db.SaveChanges();
            deleteCookie();

            return RedirectToAction("CheckOutCompleted");
        }

        public ActionResult CheckOutCompleted()
        {
            return View();
        }

        public ActionResult DeleteFromCart(int productId)
        {
            int no = 0;

            if (Request.Cookies["ShoppingCart"] != null)
            {
                string s = Request.Cookies["ShoppingCart"].Value;
                s = s.Replace(productId + " ", "");
                s = s.Replace(" " + productId, "");
                s = s.Replace(productId.ToString(), "");

                string[] products = s.Split(' ');
                no = products.Length;

                Response.Cookies["ShoppingCart"].Value = s;
                Response.Cookies["ShoppingCart"].Expires = DateTime.Now.AddDays(7);
                if (s.Equals(""))
                {
                    deleteCookie();
                    no = 0;
                }
            }

            return Content("<i class=\"fa fa-shopping-cart\"></i> Giỏ hàng (" + no + ")");
        }

        [HttpPost]
        public ActionResult QuantityChange(int idProduct, int quantity)
        {
            if (quantity > 0 && quantity < 100)
            {
                var scookie = DeleteProductFromCart(idProduct);
                var newcookie = AddProductToCart(idProduct, scookie, quantity);

                Response.Cookies["ShoppingCart"].Value = newcookie;
                Response.Cookies["ShoppingCart"].Expires = DateTime.Now.AddDays(7);

                string[] products = newcookie.Split(' ');
                int no = products.Length;
                return Content("<i class=\"fa fa-shopping-cart\"></i> Giỏ hàng (" + no + ")");
            }
            return Content("");
        }

        public ActionResult GetMoney(int idProduct)
        {
            decimal total = 0;

            List<ItemCart> model = getListItems();
            if (model == null)
            {
                return Content("0 VNĐ");
            }

            var product = model.Find(x => x.ProductId == idProduct);
            total = product.ProductPrice * product.Quantity;

            string vnd = total.ToString("0,0") + " VNĐ";
            return Content(vnd);
        }

        public ActionResult GetTotal()
        {
            decimal total = 0;

            List<ItemCart> model = getListItems();
            if (model == null)
            {
                return Content("0 VNĐ");
            }

            foreach (var item in model)
            {
                total += (item.ProductPrice * item.Quantity);
            }

            string vnd = total.ToString("0,0") + " VNĐ";
            return Content(vnd);
        }

        private List<ItemCart> getListItems()
        {
            var model = new List<ItemCart>();

            if (Request.Cookies["ShoppingCart"] != null)
            {
                string s = Request.Cookies["ShoppingCart"].Value;
                string[] ls = s.Split(' ');

                int[] lsId = Array.ConvertAll(ls, int.Parse);

                foreach (var item in lsId)
                {
                    var modelItem = model.Find(x => x.ProductId == item);
                    if (modelItem != null)
                    {
                        modelItem.Quantity += 1;
                        continue;
                    }

                    var p = db.Products.Find(item);
                    if (p != null)
                    {
                        ItemCart itemCart = new ItemCart();
                        itemCart.ProductId = p.Id;
                        itemCart.ProductName = p.Name;
                        itemCart.ProductImageUrl = p.ImageUrl;
                        itemCart.ProductPrice = p.Price;
                        itemCart.Quantity = 1;

                        model.Add(itemCart);
                    }
                }
                return model;
            }
            return null;
        }

        private void deleteCookie()
        {
            if (Request.Cookies["ShoppingCart"] != null)
            {
                Response.Cookies["ShoppingCart"].Expires = DateTime.Now.AddDays(-10);
            }
        }

        public string DeleteProductFromCart(int productId)
        {
            if (Request.Cookies["ShoppingCart"] != null)
            {
                string s = Request.Cookies["ShoppingCart"].Value;
                s = s.Replace(productId + " ", "");
                s = s.Replace(" " + productId, "");
                s = s.Replace(productId.ToString(), "");
                return s;
            }
            return "";
        }

        public string AddProductToCart(int productId, string scookie, int quantity)
        {
            string s = "";

            if (!scookie.Trim().Equals(""))
            {
                s = scookie;
                s += " " + productId;
            }
            else
            {
                s += productId;
            }

            for (int i = 0; i < quantity - 1; i++)
            {
                s += " " + productId;
            }

            return s;
        }
    }
}