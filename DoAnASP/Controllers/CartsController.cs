using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DoAnASP.Data;
using DoAnASP.Models;
using DoAnASP.wwwroot.common;

namespace DoAnASP.Controllers
{
    public class CartsController : Controller
    {
        private readonly DoAnASPContext _context;

        public CartsController(DoAnASPContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
           
            //Gán ss lauout là giỏ hàng
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "Cart");
            var checkSession = HttpContext.Session.GetString(SessionCommon.SessionAccount);
            if (checkSession == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            //List danh sách giỏ hàng theo id 
            var IdAccount = _context.Accounts.FirstOrDefault(acc => acc.Usename == checkSession).AccountId;
            var doAnASPContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c=>c.AccountId == IdAccount);
            //Tong Tien trong gio hang theo account 
            double tongtien = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.AccountId == Convert.ToInt32(IdAccount)).Sum(c => Convert.ToInt32(c.Quantity) * c.Product.Price);
            HttpContext.Session.SetString(SessionCommon.SessionTTCart, tongtien.ToString());
            //account là admin
            if (HttpContext.Session.GetString(SessionCommon.SessionAdmin) == "Admin")
            {
                var DoAnASPContext = _context.Cart.Include(c => c.Account).Include(c => c.Product);
                return View(await DoAnASPContext.ToListAsync());
            }
            else
            {
                return View(await doAnASPContext.ToListAsync());
            }
        }
        public IActionResult add(int id,string quantity)
        {
            if (HttpContext.Session.GetString(SessionCommon.SessionLayout) == "Product")
            {
                quantity = "1";
            }
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "addCarts");
            if (id != 0 && quantity != null)
            {
                HttpContext.Session.SetInt32(SessionCommon.SessionProductid, id);
                HttpContext.Session.SetString(SessionCommon.SessionQuantity, quantity);
            }
            if(id == 0 && quantity == null)
            {
                id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionCommon.SessionProductid));
                quantity = HttpContext.Session.GetString(SessionCommon.SessionQuantity);
            }    
            string username = HttpContext.Session.GetString(SessionCommon.SessionAccount);
            if (username != null)
            {
                int Acountid = _context.Accounts.FirstOrDefault(acc => acc.Usename == username).AccountId;
                Cart cart = _context.Cart.FirstOrDefault(c => c.AccountId == Acountid && c.ProductId == id);
                //List<Cart> carts = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == Acountid).ToList();
                //foreach(Cart c in carts)
                //{
                //    if(c.Product.Quantity_stock < Convert.ToInt32(c.Quantity))
                //    {
                //        ViewBag.TT = "Số Lượng không đủ";
                //        return RedirectToAction("Index", "Carts");
                //    }    
                //} 
                int ProductStock = _context.Product.FirstOrDefault(Pro => Pro.ProductId == id).Quantity_stock;
                if(Convert.ToInt64(quantity) > ProductStock)
                {
                    ViewBag.TT = "Số Lượng không đủ";
                    return RedirectToAction("Index", "Carts");
                }
                if (cart == null)
                {
                    cart = new Cart();
                    cart.ProductId = id;
                    cart.AccountId = Acountid;
                    cart.Quantity = quantity;
                    _context.Add(cart);
                    ViewBag.TT = "Thành Công mới";
                }
                else
                {
                    cart.Quantity = (Convert.ToInt32(quantity)+Convert.ToInt32(cart.Quantity)).ToString();
                    _context.Update(cart);
                    ViewBag.TT = "Thêm số lượng mới";
                }
                _context.SaveChanges();
                ViewBag.TT = "Thành Công";
                return RedirectToAction("Index", "Carts");
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }    
        }
        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }
        public IActionResult PayCarts()
        {
            //var checkSession = HttpContext.Session.GetString(SessionCommon.SessionAccount);
            //var IdAccount = _context.Accounts.FirstOrDefault(acc => acc.Usename == checkSession).AccountId;
            //var doAnASPContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == IdAccount);
            return View();
        }
        public IActionResult Pay()
        {
            var checkSession = HttpContext.Session.GetString(SessionCommon.SessionAccount);
            var IdAccount = _context.Accounts.FirstOrDefault(acc => acc.Usename == checkSession).AccountId;
            var doAnASPContext = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == IdAccount);
            //Thêm hóa đơn
            Invoice invoice = new Invoice();
            DateTime now = DateTime.Now;
            invoice.CreateDate = now;
            invoice.AccountId = IdAccount;
            invoice.Total = (_context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.AccountId == Convert.ToInt32(IdAccount)).Sum(c => Convert.ToInt32(c.Quantity) * c.Product.Price))+20000;
            _context.Add(invoice);
            _context.SaveChanges();
            //Thêm Chi tiết Hóa đơn
            List<Cart> carts = _context.Cart.Include(c => c.Account).Include(c => c.Product).Where(c => c.AccountId == IdAccount).ToList();
            foreach (Cart c in carts)
            {
                Invoicedetail invoicedetail = new Invoicedetail();
                invoicedetail.InvoiceId = invoice.InvoiceId;
                invoicedetail.ProductId = c.ProductId;
                invoicedetail.Quanty = Convert.ToInt32(c.Quantity);
                invoicedetail.Total =_context.Product.FirstOrDefault(pro => pro.ProductId == c.ProductId).Price * Convert.ToInt32(c.Quantity);
                _context.Add(invoicedetail);
                
            }
            _context.SaveChanges();
            //Tru so luong ton kho
            foreach(Cart c in carts)
            {
                c.Product.Quantity_stock -= Convert.ToInt32(c.Quantity);
                _context.Remove(c);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Carts");
        }
    }
}
