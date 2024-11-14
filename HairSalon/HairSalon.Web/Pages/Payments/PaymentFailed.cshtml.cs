using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HairSalon.Web.Pages.Payments
{
    public class PaymentFailedModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentFailedModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet()
        {
            //_httpContextAccessor.HttpContext.Session.Remove("CustomerId");
            string appointmentId = _httpContextAccessor.HttpContext.Session.GetString("AppointmentId");
        }
    }
}
