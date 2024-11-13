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

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

            // Automatically set EmployeeId from the logged-in user's claims
            int employeeId = int.Parse(User.FindFirst("EmployeeId").Value);
            Schedule.EmployeeId = employeeId;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7200/api/v1/prn231-hairsalon/schedules", Schedule);

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
