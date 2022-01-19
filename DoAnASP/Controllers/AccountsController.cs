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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DoAnASP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly DoAnASPContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountsController(DoAnASPContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            if(ViewBag.CreateTK == "True")
            {
                ViewBag.CreateTk = "Tạo tài khoản thành công";
            }    
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "Account");
            //byAdress
            var lstAdd = from acc in _context.Accounts
                         where acc.Address == "TPHCM"
                         select acc;
            //byEmail
            var accEmail = _context.Accounts.Where(accEmail => accEmail.Email.Contains("gmail")).ToList();

            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        public IActionResult Create()
        {
            return View();
        }
        // GET: Accounts/Create
        //public IActionResult Create(Account acc,string id)
        //{
        //    var KiemTraUseName = _context.Accounts.FirstOrDefault(x => x.Usename == acc.Usename);
        //    if (KiemTraUseName != null)
        //    {
        //        ViewBag.errUsername = "Tên Tài Khoản đã được sử dụng";
        //    }
        //    else
        //    {
        //        ViewBag.errUsername = "Tên Tài Khoản Hợp Lệ";
        //    }
        //    return View();
        //}
        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Usename,PassWord,Email,Name,Address,Phone,IsAdmin,avatar,ImageFile")] Account account)
        {
            ViewBag.CreateTK = null;
            if (ModelState.IsValid)
                {
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                if (account.ImageFile != null)
                {
                    //xu ly 
                    var filename = account.AccountId.ToString() + Path.GetExtension(account.ImageFile.FileName);
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "account");
                    var filePath = Path.Combine(uploadPath, filename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        account.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    account.avatar = filename;
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                }
                if(HttpContext.Session.GetString(SessionCommon.SessionAccount) == null)
                {
                    ViewBag.CreateTK = "True";
                    return RedirectToAction("Login", "Accounts");
                }
                return RedirectToAction(nameof(Index));
                }
            return RedirectToAction("Login", "Accounts");
        }
        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Usename,PassWord,Email,Name,Address,Phone,IsAdmin,avatar,ImageFile")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (account.ImageFile != null)
                    {
                        //xu ly 
                        var filename = account.AccountId.ToString() + Path.GetExtension(account.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "account");
                        var filePath = Path.Combine(uploadPath, filename);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            account.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        account.avatar = filename;
                    }
                        _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetString(SessionCommon.SessionAdmin) == null)
                {
                    return RedirectToAction("IndexAccount", "Accounts");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
        public IActionResult Login(Account login)
        {
            if(ViewBag.CreateTK == "True")
            {
                ViewBag.CreateTK = "Tạo Tài Khoản Thành Công";
            }    
            if (login.Usename != null)
            {
                if (_context.Accounts.FirstOrDefault(x => x.Usename == login.Usename) != null)
                {
                    if (login.PassWord != null)
                    {

                        if (_context.Accounts.FirstOrDefault(x => x.Usename == login.Usename && x.PassWord == login.PassWord) != null)
                        {
                            if (_context.Accounts.FirstOrDefault(x => x.Usename == login.Usename && x.PassWord == login.PassWord && x.IsAdmin == true) != null)
                            {
                                HttpContext.Session.SetString(SessionCommon.SessionAdmin, "Admin");
                            }
                            //value cho ss
                            HttpContext.Session.SetString(SessionCommon.SessionAccount, login.Usename);
                            if (HttpContext.Session.GetString(SessionCommon.SessionLayout) == "Cart")
                            {
                                return RedirectToAction("Index", "carts");
                            }
                            else
                            {
                                if(HttpContext.Session.GetString(SessionCommon.SessionLayout) == "addCarts")
                                {
                                    return RedirectToAction("add", "carts");
                                }    
                            }    
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.err = "Đăng Nhập Thất Bại! Sai Mật Khẩu !";
                        }

                    }
                    else { ViewBag.err = "Vui Lòng Nhập Mật Khẩu!"; }
                }
                else
                {
                    ViewBag.err = "Tài Khoản Không Đúng!";
                }
            }
            else
            {
                ViewBag.err = "Vui Lòng Nhập Tên Tài Khoản !";
            }
            return View();
        }
        public IActionResult LogOut()
        {
            //set null cho ss
            HttpContext.Session.Remove(SessionCommon.SessionAdmin);
            HttpContext.Session.Remove(SessionCommon.SessionAccount);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SearchAccount(string SearchAccount)
        {
            if (SearchAccount == null)
            {
                SearchAccount = "";
            }
            var lstAdd = from acc in _context.Accounts
                         where acc.Address == SearchAccount || acc.Name == SearchAccount || acc.Phone == SearchAccount || acc.Usename == SearchAccount || acc.Email == SearchAccount
                         select acc;
            return View(await lstAdd.ToListAsync());
        }
        public async Task<IActionResult> IndexAccount()
        {
            var checkSession = HttpContext.Session.GetString(SessionCommon.SessionAccount);
            var IdAccount = _context.Accounts.FirstOrDefault(acc => acc.Usename == checkSession).AccountId;
            var accid = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountId == IdAccount);
            return View(accid);
        }
    }
}
