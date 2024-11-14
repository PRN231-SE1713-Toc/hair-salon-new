using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using HairSalon.Web.Pages.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace HairSalon.Web.Pages.Payments
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentSuccessModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public int AppointmentId { get; set; }
        public IActionResult OnGet()
        {
            AppointmentId = _httpContextAccessor.HttpContext.Session.GetInt32("AppointmentId") ?? 0;
            return Page();
        }
    }
}
