using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Web.Pages.Endpoints;
using Newtonsoft.Json;

namespace HairSalon.Web.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet()
        {
            if (TempData["AppointmentData"] is string appointmentData)
            {
                Appointment = JsonConvert.DeserializeObject<Appointment>(appointmentData);
            }
            var employeeResponse = await _httpClient.GetAsync(ApplicationEndpoint.EmployeeGetAllEndPoint);
            var employees = await employeeResponse.Content.ReadFromJsonAsync<List<Employee>>();
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["StylistId"] = new SelectList(employees, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Appointments.Add(Appointment);
            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
