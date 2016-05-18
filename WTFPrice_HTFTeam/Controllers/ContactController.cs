using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class ContactController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();
        // GET: Contact

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Feedback contact)
        {
            if (User.Identity.IsAuthenticated)
            {
                contact.UserId = User.Identity.GetUserId();
            }
            else
            {
                contact.UserId = "Unknown";
            }
            contact.Status = Boolean.Parse("False");
            contact.CreatedOn = DateTime.Now;
            db.Feedbacks.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult completed()
        {
            return View();
        }
    }
}