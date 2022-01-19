using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Models
{
    public class Invoicedetail
    {
        public int InvoicedetailId { set; get; }
        public int InvoiceId { set; get; }
        public Invoice Invoice { set; get; }
        public int ProductId { set; get; }
        public Product Product { set; get; }
        public int Quanty { set; get; }
        public int Total { set; get; }
    }
}
