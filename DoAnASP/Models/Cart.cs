using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Models
{
    public class Cart
    {
        public int CartId { set; get; }
        public Account Account { set; get; }
        public int AccountId { set; get; }
        public Product Product { set; get; } 
        public int ProductId { set; get; }
        public string Quantity { set; get; }
    }
}
