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
        }

        public IList<AppointmentViewResponse> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            int? customerId = _httpContextAccessor.HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                Appointment = new List<AppointmentViewResponse>();
                return;
            }
            else
            {
                var result = await _httpClient.GetAsync(ApplicationEndpoint.AppointmentGetByCustomerIdEndPoint
                                                            + customerId.ToString() + "?status=-1");

                if (result.IsSuccessStatusCode)
                {
                    Appointment = await result.Content.ReadFromJsonAsync<List<AppointmentViewResponse>>();
                }
            }
        }
    }
}
