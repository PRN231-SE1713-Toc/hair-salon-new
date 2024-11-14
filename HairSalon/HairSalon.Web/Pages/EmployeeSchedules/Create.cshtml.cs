using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Requests;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public CreateEmployeeScheduleModel Schedule { get; set; }

        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int? employeeId = _httpContextAccessor.HttpContext?.Session.GetInt32("EmpId");

            if (employeeId == null)
            {
                ModelState.AddModelError(string.Empty, "Employee ID not found in session.");
                return Page();
            }

            Schedule.EmployeeId = employeeId.Value;

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5255/api/v1/prn231-hairsalon/schedules", Schedule);

            if (response.IsSuccessStatusCode)
            {
                Message = "Schedule created successfully!";
                return RedirectToPage("Index");
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponseModel<string>>();
                Message = errorResponse?.Message ?? "Failed to create schedule";
                return Page();
            }
        }

    }
}
