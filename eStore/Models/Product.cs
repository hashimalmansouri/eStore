using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace eStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "اسم المنتج")]
        [StringLength(200)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "السعر")]
        [Range(0, 5000)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "الوصف")]
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "الكمية")]
        [Range(0, 100)]
        public int Quantity { get; set; }
        [ScaffoldColumn(false)]
        public int TempQuantity { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateCreated { get; set; }
        //[Required(ErrorMessage = "Brand is required")]
        [Display(Name = "الماركة")]
        public int BrandId { get; set; }
        //[Required(ErrorMessage = "Genre is required")]
        [Display(Name = "الصنف")]
        public int GenreId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Genre Genre { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}