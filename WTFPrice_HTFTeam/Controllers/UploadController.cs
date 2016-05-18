using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WTFPrice_HTFTeam.Function;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UploadController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();
        Users users=new Users();
        // GET: Upload
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file, int id)
        {
            const string defaultFolderToSaveFile = "~/Upload/News/";
            if (Directory.Exists(Server.MapPath(defaultFolderToSaveFile)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(defaultFolderToSaveFile));
            }
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName);
                        var i = 1;
                        while (System.IO.File.Exists(path))
                        {
                            path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), i + "_" + fileName);
                            i++;
                        }
                        file.SaveAs(path);
                        var imageUrl = defaultFolderToSaveFile + fileName;

                            var news = db.News.Find(id);
                            news.HomeImageUrl = imageUrl;
                            db.SaveChanges();
                       
                        return RedirectToAction("Index","NewsManagement");
                    }
                }
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadUser(HttpPostedFileBase file, string id)
        {
            const string defaultFolderToSaveFile = "~/Upload/News/";
            if (Directory.Exists(Server.MapPath(defaultFolderToSaveFile)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(defaultFolderToSaveFile));
            }
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName);
                        var i = 1;
                        while (System.IO.File.Exists(path))
                        {
                            path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), i + "_" + fileName);
                            i++;
                        }
                        file.SaveAs(path);
                        var imageUrl = defaultFolderToSaveFile + fileName;
                        var user = users.GetUser(id);
                        user.Avatar = imageUrl;
                        users.GetUserManager().Update(user);

                        //return PartialView("PartialUserProfile/_PartialResultUpload", user.Avatar);
                        return RedirectToAction("Index", "UserProfile");
                    }
                }
            }
            return PartialView("PartialUserProfile/_PartialResultUpload",null);
        }
    }
}