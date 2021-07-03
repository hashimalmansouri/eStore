using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eStore.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "تاريخ الطلب")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "الكمية")]
        public int Quantity { get; set; }
        [Display(Name = "سعر الوحدة")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "اسم المنتج")]
        public string ProductName { get; set; }
    }
}