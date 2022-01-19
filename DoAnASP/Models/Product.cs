using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace DoAnASP.Models
{
    public class Product
    {
        public int ProductId { set; get; }
        public List<Invoicedetail> Invoicesdetail { get; set; }
        public ProductType ProductType { set; get; }
        public int ProductTypeId { set; get; }
        public List<Cart> Cart { set; get; }
        public string Name { set; get; }
        public string Information { set; get; }
        public int Price { set; get; }
        public int Quantity_stock { set; get; }
        public string Date { set; get; }
        public string Avatar { set; get; }
        [NotMapped]
        public IFormFile ImageFile { set; get; }
        public string SKU { set; get; }

    }
}
