using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HairSalon.Web.Pages.Payments
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentSuccessModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet()
        {
            _httpContextAccessor.HttpContext.Session.Remove("CustomerId");
            _httpContextAccessor.HttpContext.Session.Remove("AppointmentId");
        }
    }
}
