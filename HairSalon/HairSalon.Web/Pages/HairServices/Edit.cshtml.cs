using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace HairSalon.Web.Pages.HairServices
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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
            try
            {
                var service = await _httpClient.GetFromJsonAsync<HairServiceResponse>($"{ApplicationEndpoint.GetHairServiceEndpoint}/{id}");
                if (service == null)
                {
                    return NotFound();
                }
                Service = service;
                return Page();

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured while trying to fetch hair service!");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var api = ApplicationEndpoint.OtherHairServiceEndpoint + $"/{Service.Id}";
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(Service), Encoding.UTF8, "application/json");
                var result = await _httpClient.PutAsync(api, content);
                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return RedirectToPage("./Index");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"{ex.Message}");
                //return Page();
            }
            return Page();
        }
    }
}
