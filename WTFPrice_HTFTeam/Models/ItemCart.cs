using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WTFPrice_HTFTeam.Models
{
    public class ItemCart
    {
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public decimal Total
        {
            get { return this.ProductPrice * this.Quantity; }
        }
    }
}