using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Web.Pages.Endpoints;
using HairSalon.Core.Dtos.Responses;
using System.Net.Http.Headers;

namespace HairSalon.Web.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("CustomerToken") ??
                        _httpContextAccessor.HttpContext?.Session.GetString("EmpToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public IList<AppointmentViewResponse> Appointment { get; set; } = default!;
        public int EmployeeId { get; set; }

        public async Task OnGetAsync()
        {
            _httpContextAccessor.HttpContext.Session.Remove("AppointmentId");
            int? customerId = _httpContextAccessor.HttpContext?.Session.GetInt32("CustomerId");
            EmployeeId = _httpContextAccessor.HttpContext?.Session.GetInt32("EmpId") ?? 0;


            if (customerId != null)
            {
                var result = await _httpClient.GetAsync(ApplicationEndpoint.AppointmentGetByCustomerIdEndPoint + customerId.ToString() + "?status=-1");

                if (result.IsSuccessStatusCode)
                {
                    Appointment = await result.Content.ReadFromJsonAsync<List<AppointmentViewResponse>>() ?? new List<AppointmentViewResponse>();
                }
                else
                {
                    Appointment = new List<AppointmentViewResponse>();
                }
            }
            else if (EmployeeId != null)
            {
                var result = await _httpClient.GetAsync(ApplicationEndpoint.AppointmentGetAllEndPoint);

                if (result.IsSuccessStatusCode)
                {
                    Appointment = await result.Content.ReadFromJsonAsync<List<AppointmentViewResponse>>() ?? new List<AppointmentViewResponse>();
                }
                else
                {
                    Appointment = new List<AppointmentViewResponse>();
                }
            }
            else
            {
                Appointment = new List<AppointmentViewResponse>();
            }
        }
    }
}
