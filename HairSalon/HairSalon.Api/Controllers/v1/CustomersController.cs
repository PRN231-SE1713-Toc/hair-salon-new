    using Microsoft.AspNetCore.Mvc;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.PaginationDtos;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace HairSalon.Api.Controllers.v1
{
    public class CustomersController(ICustomerService customerService) : BaseApiController
    {
        private readonly ICustomerService _customerService = customerService;
        
        // GET: api/Customers
        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<List<CustomerResponse>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomers();
            if (!customers.Any()) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "No customers found!",
            });
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("customers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseModel<CustomerResponse>>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomer(id);

            if (customer == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Customer not found!"
                });
            }

            return Ok(customer);
        }
        
        [HttpPut("customer/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> UpdateCustomer(int id, UpdatedCustomer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Id does not match!"
                });
            }
            var result = await _customerService.UpdateCustomer(customer);
            if (!result)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Failed to update customer!"
                });
            }
            return NoContent();
        }
        
        [HttpPost("customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<string>> CreateCustomer(CreatedCustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid data provided"
                });
            }

            var existingCustomer = await _customerService.GetCustomers();
            var emailExist = existingCustomer.Where(e => e.Email == customer.Email).FirstOrDefault();
            if (emailExist != null)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Email is already registered"
                });
            }

            var result = await _customerService.CreateCustomer(customer);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to register!"
                });
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success!"
            });
        }

        [HttpDelete("customer/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Customer is not found!"
                });
            }

            var result = await _customerService.DeleteCustomer(customer);
            if (!result)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Failed to delete customer!"
                });
            }

            return NoContent();
        }
    }
}
