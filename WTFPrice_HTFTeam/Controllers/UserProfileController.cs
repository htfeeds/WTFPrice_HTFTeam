using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WTFPrice_HTFTeam.Function;
using WTFPrice_HTFTeam.Models;
using WTFPrice_HTFTeam.ViewModel;

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        WTFPriceEntities db = new WTFPriceEntities();
        UserProfileVM userProfileVm = new UserProfileVM();
        Users users = new Users();
       
        public ActionResult Index()
        {  string UserIdCurrent = User.Identity.GetUserId();
            if (!Request.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
            // OverView
            userProfileVm.UserPro = users.GetUser(UserIdCurrent);

            // Edit
            userProfileVm.EditUserPo = new EditUserViewModel()
            {
                Id = userProfileVm.UserPro.Id,
                Email = userProfileVm.UserPro.Email,
                UserName = userProfileVm.UserPro.UserName,
                Avatar = userProfileVm.UserPro.Avatar,
                DOB = userProfileVm.UserPro.DOB,
                PhoneNumber = userProfileVm.UserPro.PhoneNumber
            };

            // Change Password
            userProfileVm.ChangePasswordPro = new ChangePasswordViewModel();

            // Shop History
            userProfileVm.WrrapperListItemShopped=new List<ListItemShopped>();
            List<Order> orders = db.Orders.Where(x => x.UserId == UserIdCurrent).ToList();
            foreach (var itemOrder in orders)
            {
                ListItemShopped listItemShopped=new ListItemShopped(){ShopHistory = new List<ItemShopped>()};
                foreach (var itemOrderDetail in itemOrder.OrderDetails)
                {
                    Product product = itemOrderDetail.Product;
                    ItemShopped itemShopped=new ItemShopped(){OrderDate = itemOrder.CreatedOn,Name = product.Name,Quantity = itemOrderDetail.Quantity,Price = product.Price,Status =itemOrder.Status };
                    listItemShopped.ShopHistory.Add(itemShopped);
                }
                userProfileVm.WrrapperListItemShopped.Add(listItemShopped);
            }
            ViewBag.Count = userProfileVm.WrrapperListItemShopped.Count;
            return View(userProfileVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Email,Id,DOB,PhoneNumber")] EditUserViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var user = users.GetUserManager().FindById(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.UserName = editUser.UserName;
                user.Email = editUser.Email;
                user.DOB = editUser.DOB;
                user.PhoneNumber = editUser.PhoneNumber;
                users.GetUserManager().Update(user);
                return RedirectToAction("Index");
            }
            return null;
        }

        //public static async void UpdateLastvisited(ApplicationUser user)
        //{
        //    user.LastVisited = DateTime.Now;
        //    await Users.GetUserManager().UpdateAsync(user);
        //} 

    }
}