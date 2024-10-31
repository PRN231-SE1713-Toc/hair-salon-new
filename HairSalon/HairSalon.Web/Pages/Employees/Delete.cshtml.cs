using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;

namespace HairSalon.Web.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly HairSalon.Infrastructure.HairSalonDbContext _context;

        public DeleteModel(HairSalon.Infrastructure.HairSalonDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                Employee = employee;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                Employee = employee;
                _context.Employees.Remove(Employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
