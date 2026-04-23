using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCoreSolutions.Data;
using TechCoreSolutions.Models;

namespace TechCoreSolutions.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Detects if the request is coming from an AJAX call (Modal)
        /// </summary>
        private bool IsAjaxRequest => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        // GET: Employees
        public async Task<IActionResult> Index(string searchString, string department)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.FirstName.Contains(searchString) || e.LastName.Contains(searchString) || e.Position.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentDept"] = department;
            ViewBag.Departments = await _context.Employees.Select(e => e.Department).Distinct().ToListAsync();

            return View(await query.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Payrolls)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null) return NotFound();

            if (IsAjaxRequest)
                return PartialView("Details", employee);

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            if (IsAjaxRequest)
                return PartialView("Create");

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,FirstName,LastName,Position,Department,DailyRate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            if (IsAjaxRequest)
                return PartialView("Edit", employee);

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,FirstName,LastName,Position,Department,DailyRate")] Employee employee)
        {
            if (id != employee.EmployeeID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            // Returns the confirmation modal content
            if (IsAjaxRequest)
                return PartialView("Delete", employee);

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}