using Microsoft.AspNetCore.Mvc;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;

namespace HairSalon.Api.Controllers.v1
{
    public class CustomersController(ICustomerService customerService) : BaseApiController
    {
        private readonly ICustomerService _customerService = customerService;
        
        // GET: api/Customers
        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponseModel<List<CustomerResponse>>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomers();
            if (!customers.Any()) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "No customers found!",
            });
            return Ok(new ApiResponseModel<List<CustomerResponse>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Fetch data successfully!",
                Response = customers
            });
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

            return Ok(new ApiResponseModel<CustomerResponse>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Fetch data successfully!",
                Response = customer
            });
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("customer/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCustomer(int id, UpdatedCustomer customer)
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
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Success!"
            });
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateCustomer(CreatedCustomerModel customer)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _customerService.CreateCustomer(customer);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Failed to create customer!"
                });
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Success!"
            });
        }

        // DELETE: api/Customers/5
        [HttpDelete("customer/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCustomer(int id)
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

            return Ok(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Success!"
            });
        }
    }
}
