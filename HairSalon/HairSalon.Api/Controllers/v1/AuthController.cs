using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HairSalon.Api.Controllers.v1
{
    public class AuthController(
        ICustomerService customerService,
        IEmployeeService employeeService) : BaseApiController
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly IEmployeeService _employeeService = employeeService;

        /// <summary>
        /// Authenticate for customer
        /// </summary>
        /// <param name="requestModel"></param>
        [HttpPost("customer/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<ApiResponseModel<LoginCustomerResponse>>> LoginForCustomer([FromBody] LoginRequest requestModel)
        {
            var customer = await _customerService.CheckLoginForCustomer(requestModel.Email, requestModel.Password);
            if (customer is null) return NotFound(new ApiResponseModel<string>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "User not found"
            });
           
            return Ok(new ApiResponseModel<LoginCustomerResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success",
                Response = customer
            });
        }

        /// <summary>
        /// Authenticate for employee
        /// </summary>
        /// <param name="requestModel"></param>
        [HttpPost("employee/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<ActionResult<ApiResponseModel<LoginEmployeeResponse>>> LoginForEmployee([FromBody] LoginRequest requestModel)
        {
            var employee = await _employeeService.CheckLoginForEmployee(requestModel.Email, requestModel.Password);
            if (employee is null) return NotFound(new ApiResponseModel<string>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "User not found"
            });

            return Ok(new ApiResponseModel<LoginEmployeeResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success",
                Response = employee,
            });
        }
    }
}
