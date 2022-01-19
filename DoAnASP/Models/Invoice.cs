using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Models
{
    public class Invoice
    {
        public List<Invoicedetail> Invoicesdetail { get; set; }
        public int InvoiceId { set; get; }
        public int AccountId { set; get; }
        public Account Account { get; set; }
        public DateTime CreateDate { set; get; }
        public int Total { set; get; }
    }
}
