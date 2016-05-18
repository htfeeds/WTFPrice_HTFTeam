using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.ViewModel
{
    public class NewsVM
    {
        public List<News> SortByViews { get; set; }
        public List<News> SortByCreatedDate { get; set; }
        public Repository SortByCategory { get; set; }
    }

    public class Repository
    {
        public List<News> SortByCategoryTech { get; set; }
        public List<News> SortByCategoryEvaluate { get; set; }
        public List<News> SortByCategoryDiscount { get; set; }
    }

    public class UserProfileVM
    {
        public ApplicationUser UserPro { get; set; }
        public EditUserViewModel EditUserPo { get; set; }
        public ChangePasswordViewModel ChangePasswordPro { get; set; }
        public List<ListItemShopped> WrrapperListItemShopped { get; set; }

    }

    public class ListItemShopped
    {
        public List<ItemShopped> ShopHistory { get; set; }
    }

    public class ItemShopped
    {
        public DateTime OrderDate { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public bool Status { get; set; }

        public decimal SumPrice
        {
            get { return this.Price * this.Quantity; }
        }
    }
}