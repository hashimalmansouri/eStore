using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eStore.Models
{
    public class Genre
    {
        [ScaffoldColumn(false)]
        public int GenreId { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "اسم الصنف")]
        public string GenreName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}