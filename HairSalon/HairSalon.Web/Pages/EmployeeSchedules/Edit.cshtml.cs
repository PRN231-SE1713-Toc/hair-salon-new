using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class EditModel : PageModel
    {
        private readonly HairSalon.Infrastructure.HairSalonDbContext _context;

        public EditModel(HairSalon.Infrastructure.HairSalonDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EmployeeSchedule EmployeeSchedule { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeschedule = await _context.EmployeeSchedules.FirstOrDefaultAsync(m => m.Id == id);
            if (employeeschedule == null)
            {
                return NotFound();
            }

            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (employeeIdClaim == null || employeeschedule.EmployeeId != int.Parse(employeeIdClaim))
            {
                return NotFound();
            }

            EmployeeSchedule = employeeschedule;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (employeeIdClaim == null || EmployeeSchedule.EmployeeId != int.Parse(employeeIdClaim))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EmployeeSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeScheduleExists(EmployeeSchedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EmployeeScheduleExists(int id)
        {
            return _context.EmployeeSchedules.Any(e => e.Id == id);
        }
    }
}
