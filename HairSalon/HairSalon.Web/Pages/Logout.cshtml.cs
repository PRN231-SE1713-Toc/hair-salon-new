using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace HairSalon.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            // Logout implementation: Removes the data in addition with session cookie from the browser
            // INFO: Reference code for studying
            //_httpContextAccessor.HttpContext.SignOutAsync()
            
            _httpContextAccessor.HttpContext?.Session.Clear();

            if (_httpContextAccessor.HttpContext is not null)
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(".AspNetCore.Session");
            return RedirectToPage("/Index");
        }
    }
}
