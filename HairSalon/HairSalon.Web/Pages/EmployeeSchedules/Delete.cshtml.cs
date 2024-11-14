using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Responses;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public EmployeeScheduleResponse EmployeeSchedule { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5255/api/v1/prn231-hairsalon/schedule/{id}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseModel<EmployeeScheduleResponse>>();
                EmployeeSchedule = apiResponse?.Response;
            }
            else
            {
                Message = "Schedule not found.";
                return RedirectToPage("/EmployeeSchedules/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5255/api/v1/prn231-hairsalon/schedule/{id}");

            if (response.IsSuccessStatusCode)
            {
                Message = "Schedule deleted successfully.";
            }
            else
            {
                Message = "Failed to delete schedule.";
            }

            return RedirectToPage("/EmployeeSchedules/Index");
        }
    }

}
