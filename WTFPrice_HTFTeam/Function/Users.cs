using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Function
{
    public  class Users
    {
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())); 
        public UserManager<ApplicationUser> GetUserManager()
        {
            return userManager;
        } 
        public  ApplicationUser GetUser(string userId)
        {
            return GetUserManager().FindById(userId);
        }
    }
}