﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WTFPrice_HTFTeam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        public Product()
        {
            this.Comments = new HashSet<Comment>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Product_Category_Mappings = new HashSet<Product_Category_Mappings>();
            this.ProductDetails = new HashSet<ProductDetail>();
            this.ProductImages = new HashSet<ProductImage>();
            this.ProductReviews = new HashSet<ProductReview>();
            this.RelatedProducts = new HashSet<RelatedProduct>();
        }

        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Thông tin sản phẩm")]
        public string Info { get; set; }

        [Display(Name = "Bảo hành")]
        public string Warranty { get; set; }

        [Display(Name = "Khuyến mãi")]
        public string Promotion { get; set; }

        public string ImageUrl { get; set; }

        public int ManufacturerId { get; set; }

        [Display(Name = "Home")]
        public bool ShowOnHomePage { get; set; }

        public string GalleryImageUrl { get; set; }

        [Display(Name = "Gallery")]
        public bool GalleryActived { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public System.DateTime CreatedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Product_Category_Mappings> Product_Category_Mappings { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<RelatedProduct> RelatedProducts { get; set; }
    }
}
