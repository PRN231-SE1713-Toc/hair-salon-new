using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HairSalon.Web.Pages.HairServices
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

        public HairServiceResponse Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // TODO: Add authorization
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var api = ApplicationEndpoint.GetHairServiceEndpoint + $"/{id}";
                var response = await _httpClient.GetFromJsonAsync<HairServiceResponse>(api);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    Service = response;
                }
                return Page();
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occured while trying to fetch hair service! {ex.Message}");
                return Page();
            }
        }
    }
}
