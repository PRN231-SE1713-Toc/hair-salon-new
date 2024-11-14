using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace HairSalon.Web.Pages.Admins.DashBoard
{
    public class ServiceModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ServiceModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public List<Service> Services { get; set; } = new List<Service>();

        [BindProperty]
        public Service NewService { get; set; } = new Service();

        public ServiceModel(HttpClient httpClient, ILogger<ServiceModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("CustomerToken") ??
                        _httpContextAccessor.HttpContext?.Session.GetString("EmpToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApplicationEndpoint.DeleteHairServiceEndpoint}{id}");

                if (response.IsSuccessStatusCode)
                {
                    await FetchServicesAsync();
                }
                else
                {
                    _logger.LogWarning("Unable to delete service, Status Code: {StatusCode}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, "Unable to delete service. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the service.");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the service. Please try again.");
            }

            return Page();
        }

        public async Task OnGetAsync()
        {
            await FetchServicesAsync();
        }

        private async Task FetchServicesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApplicationEndpoint.GetHairServiceEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Services = JsonSerializer.Deserialize<List<Service>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Service>();
                }
                else
                {
                    _logger.LogWarning("Unable to fetch services, Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the services.");
            }
        }
    }
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EstimatedDuration { get; set; }
        public decimal Price { get; set; }
    }
}