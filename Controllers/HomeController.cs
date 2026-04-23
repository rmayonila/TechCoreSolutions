using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechCoreSolutions.Data;
using TechCoreSolutions.Models;

namespace TechCoreSolutions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Count total employees (SQL can handle this directly)
            ViewBag.EmployeeCount = await _context.Employees.CountAsync();

            // 2. Load payrolls into memory to calculate the NetPay sum
            // We use .Include(e => e.Employee) because NetPay needs the DailyRate
            var allPayrolls = await _context.Payrolls
                .Include(p => p.Employee)
                .ToListAsync();

            // 3. Calculate total payout using C# (since NetPay is a calculated property)
            ViewBag.TotalPayroll = allPayrolls.Sum(p => p.NetPay);

            // 4. Get the 5 most recent records for the dashboard table
            ViewBag.RecentPayrolls = allPayrolls
                .OrderByDescending(p => p.Date)
                .Take(5)
                .ToList();

            return View();
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