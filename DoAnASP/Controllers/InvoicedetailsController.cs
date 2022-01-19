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
    public class InvoicedetailsController : Controller
    {
        private readonly DoAnASPContext _context;

        public InvoicedetailsController(DoAnASPContext context)
        {
            _context = context;
        }

        // GET: Invoicedetails
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "Invoicedetails");
            var doAnASPContext = _context.Invoicedetail.Include(i => i.Invoice).Include(i => i.Product);
            return View(await doAnASPContext.ToListAsync());
        }

        // GET: Invoicedetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoicedetail = await _context.Invoicedetail
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.InvoicedetailId == id);
            if (invoicedetail == null)
            {
                return NotFound();
            }

            return View(invoicedetail);
        }

        // GET: Invoicedetails/Create
        public IActionResult Create()
        {
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: Invoicedetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoicedetailId,InvoiceId,ProductId,Quanty,Total")] Invoicedetail invoicedetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoicedetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoicedetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", invoicedetail.ProductId);
            return View(invoicedetail);
        }

        // GET: Invoicedetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoicedetail = await _context.Invoicedetail.FindAsync(id);
            if (invoicedetail == null)
            {
                return NotFound();
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoicedetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", invoicedetail.ProductId);
            return View(invoicedetail);
        }

        // POST: Invoicedetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoicedetailId,InvoiceId,ProductId,Quanty,Total")] Invoicedetail invoicedetail)
        {
            if (id != invoicedetail.InvoicedetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoicedetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicedetailExists(invoicedetail.InvoicedetailId))
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
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoicedetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", invoicedetail.ProductId);
            return View(invoicedetail);
        }

        // GET: Invoicedetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoicedetail = await _context.Invoicedetail
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.InvoicedetailId == id);
            if (invoicedetail == null)
            {
                return NotFound();
            }

            return View(invoicedetail);
        }

        // POST: Invoicedetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoicedetail = await _context.Invoicedetail.FindAsync(id);
            _context.Invoicedetail.Remove(invoicedetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoicedetailExists(int id)
        {
            return _context.Invoicedetail.Any(e => e.InvoicedetailId == id);
        }
    }
}
