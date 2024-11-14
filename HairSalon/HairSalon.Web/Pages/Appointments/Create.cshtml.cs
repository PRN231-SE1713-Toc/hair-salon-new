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
using static System.Runtime.InteropServices.JavaScript.JSType;
using HairSalon.Core.Dtos.Requests;

namespace HairSalon.Web.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public AppointmentCreateModel Appointment { get; set; } = new AppointmentCreateModel();

        public List<Employee> availableStylist = new List<Employee>();

        public List<Core.Entities.Service> selectedServices = new List<Core.Entities.Service>();

        public CreateModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet()
        {
            await Populate();
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await Populate();
            Appointment.AppointmentStatus = 0;
            Appointment.AppointmentServices = selectedServices.Select(s => new CreateAppointmentServiceModel
            {
                ServiceId = s.Id,
            }).ToList();
            if (!ModelState.IsValid)
            {
                await Populate();
                ModelState.AddModelError(string.Empty, "Invalid");
                return Page();
            }
            string token = _httpContextAccessor.HttpContext.Session.GetString("CustomerToken");
            _httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await _httpClient.PostAsync(ApplicationEndpoint.AppointmentCreateEndPoint, JsonContent.Create(Appointment));
            if (result.StatusCode == System.Net.HttpStatusCode.Created) 
            {
                var createdAppointment = await result.Content.ReadFromJsonAsync<AppointmentCreateModel>();

                // Lưu CustomerId và AppointmentId vào session
                _httpContextAccessor.HttpContext.Session.SetInt32("CustomerId", createdAppointment.CustomerId);
                _httpContextAccessor.HttpContext.Session.SetInt32("AppointmentId", createdAppointment.Id);

                return RedirectToPage("/Payments/PaymentConfirmation");
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Populate();
                ModelState.AddModelError(string.Empty, "Unauthorized");
                return Page();
            }
            else
            {
                await Populate();
                ModelState.AddModelError(string.Empty, "Create failed");
                return Page();
            }
        }

        private async Task Populate()
        {
            Appointment.CustomerId = _httpContextAccessor.HttpContext.Session.GetInt32("CustomerId") ?? 0;
            if ((TempData["AppointmentDate"] != null)
                && (TempData["AppointmentDate"] != null)
                && (TempData["AppointmentDate"] != null))
            {
                string appointmentDateString = JsonConvert.DeserializeObject<string>(TempData["AppointmentDate"]
                                                .ToString());
                if (!string.IsNullOrEmpty(appointmentDateString))
                {
                    DateOnly appointmentDate;
                    DateOnly.TryParse(appointmentDateString, out appointmentDate);
                    Appointment.AppointmentDate = appointmentDate;
                }
                string startTimeString = JsonConvert.DeserializeObject<string>(TempData["AppointmentStartTime"]
                                            .ToString());
                if (!string.IsNullOrEmpty(startTimeString))
                {
                    TimeOnly startTime;
                    TimeOnly.TryParse(startTimeString, out startTime);
                    Appointment.StartTime = startTime;
                }
                string endTimeString = JsonConvert.DeserializeObject<string>(TempData["AppointmentEndTime"]
                                        .ToString());
                if (!string.IsNullOrEmpty(endTimeString))
                {
                    TimeOnly endTime;
                    TimeOnly.TryParse(endTimeString, out endTime);
                    Appointment.EndTime = endTime;
                }
            }
            var result = await _httpClient.GetAsync(ApplicationEndpoint.GetHairServiceEndpoint);
            if (result != null)
            {
                var selectedServiceIds = JsonConvert.DeserializeObject<List<int>>(TempData["selectedServiceIds"].ToString());
                var services = result.Content.ReadFromJsonAsync<List<Core.Entities.Service>>().Result;
                selectedServices = services.Where(service => selectedServiceIds.Contains(service.Id)).ToList();
            }
            //result = await _httpClient.GetAsync(ApplicationEndpoint.EmployeeGetAvailableEndPoint
            //    + Appointment.AppointmentDate
            //    + "/Stylists"
            //    + "?startTime=" + Appointment.StartTime + "&endTime=" + Appointment.EndTime
            //    );
            result = await _httpClient.GetAsync(ApplicationEndpoint.EmployeeGetAllEndPoint);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stylists = result.Content.ReadFromJsonAsync<List<Employee>>().Result;
                availableStylist = stylists;
                ViewData["StylistId"] = new SelectList(stylists, "Id", "Name");
            } else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }
            TempData["AppointmentDate"] = TempData["AppointmentDate"];
            TempData["AppointmentStartTime"] = TempData["AppointmentStartTime"];
            TempData["AppointmentEndTime"] = TempData["AppointmentEndTime"];
            TempData["selectedServiceIds"] = TempData["selectedServiceIds"];
        }
    }
}
