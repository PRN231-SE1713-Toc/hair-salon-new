using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HairSalon.Web.Pages.HairServices
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

        public IList<HairServiceResponse> HairServices { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // TODO: Add authorization
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IList<HairServiceResponse>>(ApplicationEndpoint.GetHairServiceEndpoint);
                if (response is not null)
                {
                    HairServices = response;
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occured while trying to fetch hair services! {ex.Message}");
            }
        }
    }

    public class HairServiceResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? EstimatedDuration { get; set; }

        public decimal Price { get; set; }
    }
}
