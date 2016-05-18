using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WTFPrice_HTFTeam.Models
{
    public class ProductMetadata
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id;

        [Display(Name = "Tên sản phẩm")]
        public string Name;

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Display(Name = "Giá")]
        public decimal Price;

        public int Stock;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Thông tin sản phẩm")]
        public string Info;

        [Display(Name = "Bảo hành")]
        public string Warranty;

        [Display(Name = "Khuyến mãi")]
        public string Promotion;

        public string ImageUrl;

        public int ManufacturerId;

        [Display(Name = "Home")]
        public bool ShowOnHomePage;

        public string GalleryImageUrl;

        [Display(Name = "Gallery")]
        public bool GalleryActived;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public System.DateTime CreatedOn;
    }

    public class OrderMetada
    {
        public int Id;
        public string UserId;
        public string ContactName;
        public string Email;
        public string ContactAddress;
        public string ContactPhone;
        public bool Status;
        [Display(Name = "Message")]
        public string Note;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public System.DateTime CreatedOn;
    }
}