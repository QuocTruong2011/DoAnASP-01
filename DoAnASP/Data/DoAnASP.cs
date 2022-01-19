using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Models;

namespace DoAnASP.Data
{
    public class DoAnASPContext : DbContext
    {
        public DoAnASPContext(DbContextOptions<DoAnASPContext> options)
        : base(options)
        {
        }
        public DbSet<DoAnASP.Models.Account> Accounts { get; set; }
        public DbSet<DoAnASP.Models.Invoice> Invoice { get; set; }
        public DbSet<DoAnASP.Models.Invoicedetail> Invoicedetail { get; set; }
        public DbSet<DoAnASP.Models.Product> Product { get; set; }
        public DbSet<DoAnASP.Models.Cart> Cart { set; get; }
        public DbSet<DoAnASP.Models.ProductType> ProductType { set; get; }
    }
}
