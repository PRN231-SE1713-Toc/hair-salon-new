using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace HairSalon.Web.Pages.Admins.DashBoard
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public Service Service { get; set; }

        public EditModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync(ApplicationEndpoint.GetHairServiceByIdEndpoint + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Service = JsonSerializer.Deserialize<Service>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Service();
                if (!string.IsNullOrEmpty(Service.EstimatedDuration))
                {
                    Service.EstimatedDuration = Service.EstimatedDuration.Replace(" minutes", "");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var serviceJson = JsonSerializer.Serialize(Service);
            Console.WriteLine($"Request JSON: {serviceJson}");

            var content = new StringContent(serviceJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(ApplicationEndpoint.UpdateHairServiceEndpoint + Service.Id, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Unable to update service. Please try again.");
                return Page();
            }

            return RedirectToPage("/Admins/DashBoard/Service");
        }
    }
}

