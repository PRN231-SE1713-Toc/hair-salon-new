using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class DeleteModel : PageModel
    {
        private readonly HairSalon.Infrastructure.HairSalonDbContext _context;

        public DeleteModel(HairSalon.Infrastructure.HairSalonDbContext context)
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

            var employeeSchedule = await _context.EmployeeSchedules.FirstOrDefaultAsync(m => m.Id == id);

            if (employeeSchedule == null)
            {
                return NotFound();
            }
            else
            {
                EmployeeSchedule = employeeSchedule;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeSchedule = await _context.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule != null)
            {
                EmployeeSchedule = employeeSchedule;
                _context.EmployeeSchedules.Remove(EmployeeSchedule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
