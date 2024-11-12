﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly HairSalon.Infrastructure.HairSalonDbContext _context;

        public DetailsModel(HairSalon.Infrastructure.HairSalonDbContext context)
        {
            _context = context;
        }

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
            else
            {
                EmployeeSchedule = employeeschedule;
            }
            return Page();
        }
    }
}