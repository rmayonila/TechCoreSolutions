using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechCoreSolutions.Data;
using TechCoreSolutions.Models;

namespace TechCoreSolutions.Controllers
{
    public class PayrollsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayrollsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAjaxRequest => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        // GET: Payrolls
        public async Task<IActionResult> Index(int? employeeId, int? month)
        {
            var query = _context.Payrolls.Include(p => p.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                query = query.Where(p => p.EmployeeID == employeeId);
            }

            if (month.HasValue)
            {
                query = query.Where(p => p.Date.Month == month);
            }

            ViewBag.Employees = await _context.Employees.ToListAsync();
            ViewData["CurrentEmployee"] = employeeId;
            ViewData["CurrentMonth"] = month;

            var payrolls = await query.ToListAsync();
            return View(payrolls);
        }

        // GET: Payrolls/ByEmployee/5
        public async Task<IActionResult> ByEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var payrolls = await _context.Payrolls
                .Where(p => p.EmployeeID == id)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            ViewData["EmployeeName"] = employee.FullName;
            ViewData["EmployeeID"] = id;
            return View(payrolls);
        }

        // GET: Payrolls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollID == id);
            
            if (payroll == null)
            {
                return NotFound();
            }

            if (IsAjaxRequest)
                return PartialView("Details", payroll);

            return View(payroll);
        }

        // GET: Payrolls/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await _context.Employees.ToListAsync();
            
            if (IsAjaxRequest)
                return PartialView("Create");

            return View();
        }

        // POST: Payrolls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayrollID,EmployeeID,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(payroll);
        }

        // GET: Payrolls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll == null)
            {
                return NotFound();
            }
            ViewBag.Employees = await _context.Employees.ToListAsync();

            if (IsAjaxRequest)
                return PartialView("Edit", payroll);

            return View(payroll);
        }

        // POST: Payrolls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayrollID,EmployeeID,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            if (id != payroll.PayrollID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payroll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollExists(payroll.PayrollID))
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
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(payroll);
        }

        // GET: Payrolls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollID == id);
            if (payroll == null)
            {
                return NotFound();
            }

            if (IsAjaxRequest)
                return PartialView("Delete", payroll);

            return View(payroll);
        }

        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll != null)
            {
                int employeeId = payroll.EmployeeID;
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PayrollExists(int id)
        {
            return _context.Payrolls.Any(e => e.PayrollID == id);
        }
    }
}
