using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eStore.Models
{
    public class Brand
    {
        [ScaffoldColumn(false)]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "اسم الماركة")]
        public string BrandName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}