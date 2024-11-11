using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace HairSalon.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<IndexModel> _logger;

        public List<Service> Services { get; set; } = new List<Service>();

        public IndexModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGet()
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
        public class Service
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string EstimatedDuration { get; set; }
            public decimal Price { get; set; }
        }
    }
}
