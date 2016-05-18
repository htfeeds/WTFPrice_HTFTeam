using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    public class CommentsController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        [HttpPost]
        public ActionResult PostComment(Comment comment, int productId)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    comment.UserId = User.Identity.GetUserId();
                }
                else
                {
                    comment.UserId = "Unknown";
                }
                comment.CreatedOn = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();

                return PartialView("_CommentPartial", comment);
            }
            return Content("");
        }
    }
}