using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Commons;
using System.Net;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public EmployeeScheduleResponse? Schedule { get; set; }
        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponseModel<EmployeeScheduleResponse>>($"https://localhost:7200/api/v1/prn231-hairsalon/schedules/{id}");

            if (response != null && response.StatusCode == HttpStatusCode.OK && response.Response != null)
            {
                Schedule = response.Response;
                Message = response.Message;
            }
            else
            {
                Message = response?.Message ?? "Schedule not found!";
            }
        }
    }
}
