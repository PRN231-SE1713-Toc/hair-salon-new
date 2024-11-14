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
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<EmployeeScheduleResponse> Schedules { get; set; } = new();
        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5255/api/v1/prn231-hairsalon/schedules");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseModel<List<EmployeeScheduleResponse>>>();

                if (apiResponse.Response != null)
                {
                    Schedules = apiResponse.Response;
                    Message = apiResponse.Message;
                    Console.WriteLine("Schedules fetched successfully.");
                }
                else
                {
                    Message = "No schedules found!";
                    Console.WriteLine("No schedules found in response.");
                }
            }
            else
            {
                Message = "Failed to load schedules.";
                Console.WriteLine("Failed to load schedules, API call unsuccessful.");
            }
        }
    }
}
