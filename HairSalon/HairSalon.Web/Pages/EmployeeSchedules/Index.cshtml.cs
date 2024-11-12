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
    public class IndexModel : PageModel
    {
        private readonly HairSalon.Infrastructure.HairSalonDbContext _context;

        public IndexModel(HairSalon.Infrastructure.HairSalonDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeSchedule> EmployeeSchedule { get; set; } = default!;

        public async Task OnGetAsync()
        {
            EmployeeSchedule = await _context.EmployeeSchedules.ToListAsync();
        }
    }
}
