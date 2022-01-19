using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnASP.Data;
using DoAnASP.Models;
using Microsoft.AspNetCore.Http;
using DoAnASP.wwwroot.common;

namespace DoAnASP.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly DoAnASPContext _context;

        public InvoicesController(DoAnASPContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "Invoices");
            ////ByTotalRange
            //var lstProducts = from invP in _context.Invoice
            //                    where invP.Total >= 400000 && invP.Total <= 600000
            //                    select invP;
            ////theo ten 
            //var invName = _context.Invoice.Include(i => i.Account).Where(inv => inv.Account.Usename.Contains("john")).ToList();
            ////theo dia chi
            //var invAdd = _context.Invoice.Include(i => i.Account).Where(inv => inv.Account.Address.Contains("TPHCM")).ToList();
            ////theo san pham
            //double avg = _context.Invoice.Average(inv => inv.Total);
            //double maxInvoice = _context.Invoice.Max(Inv => Inv.Total);
            ////Hoa Don tren 3000
            //var inv = _context.Invoice.Where(x => x.Total > 3000).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            ////
            var doAnASPContext = _context.Invoice.Include(i => i.Account);
            return View(await doAnASPContext.ToListAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //    //ByTotalRange
        //    var lstProducts = from inv in _context.Invoice
        //                      where inv.Total >= 400000 && inv.Total <= 600000
        //                      select inv;
        //    //theo ten 
        //    var invName = _context.Invoice.Include(i => i.Account).Where(inv => inv.Account.Usename.Contains("john")).ToList();
        //    //theo dia chi
        //    var invAdd = _context.Invoice.Include(i => i.Account).Where(inv => inv.Account.Address.Contains("TPHCM")).ToList();
        //    //theo san pham
        //    var doAnASPContext = _context.Invoice.Include(i => i.Account);
        //    return View(doAnASPContext.ToListAsync());
        //}
        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,AccountId,CreateDate,Total")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", invoice.AccountId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", invoice.AccountId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,AccountId,CreateDate,Total")] Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", invoice.AccountId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Account)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceId == id);
        }
    }
}
