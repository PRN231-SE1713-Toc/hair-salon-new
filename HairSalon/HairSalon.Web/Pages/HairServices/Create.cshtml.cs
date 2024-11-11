using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HairSalon.Web.Pages.Endpoints;

namespace HairSalon.Web.Pages.HairServices
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // TODO: Handle data loss when creating a service but it is failed
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HairServiceModel Service { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            // TODO: Add authorization
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApplicationEndpoint.OtherHairServiceEndpoint, Service);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create hair service! {ex.Message}");
            }
            return Page();
        }
    }

    public class HairServiceModel
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Duration { get; set; }

        public decimal Price { get; set; }
    }
}
