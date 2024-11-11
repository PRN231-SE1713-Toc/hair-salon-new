using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HairSalon.Web.Pages.HairServices
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public DeleteModel(
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        [BindProperty]
        public HairServiceResponse Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // TODO: Add authorization
            if (id == null)
            {
                return NotFound();
            }
            var api = ApplicationEndpoint.GetHairServiceEndpoint + $"/{id}";
            try
            {
                var service = await _httpClient.GetFromJsonAsync<HairServiceResponse>(api);
                if (service == null)
                {
                    return NotFound();
                }
                else
                {
                    Service = service;
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occured while trying to fetch hair service! {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _httpClient.DeleteAsync(ApplicationEndpoint.OtherHairServiceEndpoint + $"/{id}");
            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occured while trying to delete hair service!");
                return await OnGetAsync(id);
            }
        }
    }
}
