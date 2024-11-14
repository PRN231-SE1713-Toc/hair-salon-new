using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Requests;
using System.Text.Json;

namespace HairSalon.Web.Pages.EmployeeSchedules
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public UpdateEmployeeSchedule Schedule { get; set; } = new();

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponseModel<UpdateEmployeeSchedule>>($"http://localhost:5255/api/v1/prn231-hairsalon/schedules/{id}");

            if (response?.Response != null)
            {
                Schedule = response.Response;
            }
            else
            {
                Message = "Schedule not found!";
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5255/api/v1/prn231-hairsalon/schedule/{id}", Schedule);

            var rawContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response Status Code: " + response.StatusCode);
            Console.WriteLine("Response Content: " + rawContent);

            if (!response.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(rawContent))
                {
                    Message = "The API response is empty. Please check the API or parameters.";
                    Console.WriteLine("Empty response received.");
                    return Page();
                }

                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponseModel<UpdateEmployeeSchedule>>();
                    Message = errorResponse?.Message ?? "Failed to update schedule";
                    Console.WriteLine("Error Response: " + errorResponse?.Message);
                }
                catch (JsonException ex)
                {
                    Message = "Error parsing the response: " + ex.Message;
                    Console.WriteLine("JSON Parsing Error: " + ex.Message);
                }

                return Page();
            }

            Message = "Schedule updated successfully!";

            return RedirectToPage("/EmployeeSchedules/Index");
        }

    }
}
