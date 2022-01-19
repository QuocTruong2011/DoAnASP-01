using DoAnASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DoAnASP.wwwroot.common;
using DoAnASP.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DoAnASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DoAnASPContext _context;

        public HomeController(ILogger<HomeController> logger,DoAnASPContext context)
        {

            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString(SessionCommon.SessionLayout, "Home");
            var p = _context.ProductType.Include(p => p.Product);
            return View(await p.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
