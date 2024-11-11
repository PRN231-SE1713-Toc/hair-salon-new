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
using HairSalon.Web.Pages.Endpoints;

namespace HairSalon.Web.Pages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppointmentViewResponse Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _httpClient.GetAsync(ApplicationEndpoint.AppointmentGetByIdEndPoint + id);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = await appointment.Content.ReadFromJsonAsync<AppointmentViewResponse>();
            }
            return Page();
        }
    }
}
