using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eStore.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [DisplayName("الأسم الأول")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [DisplayName("الأسم الأخير")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [DisplayName("البريد الإلكتروني")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "البريد غير صالح.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}