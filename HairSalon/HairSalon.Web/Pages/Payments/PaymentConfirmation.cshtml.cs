using HairSalon.Core.Dtos.Responses;
using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace HairSalon.Web.Pages.Payments
{
    public class PaymentConfirmationModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public PaymentConfirmationModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public AppointmentViewResponse appointment {  get; set; }
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            await populate();
            if (CustomerId == 0 || AppointmentId == 0)
            {
                ModelState.AddModelError(string.Empty, "Appointment Not found.");
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await populate();

            if (CustomerId == 0 || AppointmentId == 0)
            {
                ModelState.AddModelError(string.Empty, "Appointment detail not found.");
                return Page();
            }

            var url = $"{ApplicationEndpoint.PaymentVNPayEndpoint}?appointmentId={AppointmentId}&userId={CustomerId}";
            var response = await _httpClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<PaymentResponseModel>();
                if (data != null && data.url != null && !string.IsNullOrEmpty(data.url.result))
                {
                    return Redirect(data.url.result);
                }
            }
            ModelState.AddModelError(string.Empty, "Payment unsuccessfully.");
            return Page();
        }

        public async Task populate()
        {
            CustomerId = _httpContextAccessor.HttpContext.Session.GetInt32("CustomerId") ?? 0;
            AppointmentId = _httpContextAccessor.HttpContext.Session.GetInt32("AppointmentId") ?? 0;

            if (CustomerId > 0 || AppointmentId > 0)
            {
                var result = await _httpClient.GetAsync(ApplicationEndpoint.AppointmentGetByIdEndPoint + AppointmentId);
                appointment = result.Content.ReadFromJsonAsync<AppointmentViewResponse>().Result;
            }
        }

        public class PaymentResponseModel
        {
            public UrlData url { get; set; }

            public class UrlData
            {
                public string result { get; set; }
            }

            // Các thuộc tính khác có thể thêm nếu cần thiết
            public int id { get; set; }
            public int status { get; set; }
            public bool isCanceled { get; set; }
            public bool isCompleted { get; set; }
        }
    }
}
