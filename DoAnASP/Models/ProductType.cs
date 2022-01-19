using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Models
{
    public class ProductType
    {
        public int ProductTypeId { set; get; }
        public List<Product> Product { set; get; }
        public string Name { set; get; }
        public bool Status { set; get; }
        public string Author { set; get; }
    }
}
