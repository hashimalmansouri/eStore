using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eStore.Models;
using eStore.ViewModels;

namespace eStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int Id { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}