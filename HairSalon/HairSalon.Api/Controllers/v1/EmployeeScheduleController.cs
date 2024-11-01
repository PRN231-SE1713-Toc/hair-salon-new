using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Api.Controllers.v1
{
    public class EmployeeScheduleController : BaseApiController
    {
        private readonly HairSalonDbContext _context;
        private readonly IEmployeeScheduleService _employeeScheduleService;

        public EmployeeScheduleController(HairSalonDbContext context, IEmployeeScheduleService employeeScheduleService)
        {
            _context = context;
            _employeeScheduleService = employeeScheduleService;
        }

        //GET: api/Schedule
        [HttpGet("schedules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleResponse>>> GetSchedule()
        {
            var schedule =  await _employeeScheduleService.GetSchedule();
            if (!schedule.Any()) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "No customers found!",
            });
            return Ok(new ApiResponseModel<List<EmployeeScheduleResponse>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Fetch data successfully!",
                Response = schedule
            });
        }

        [HttpGet("employee/{id}/schedules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleResponse>>> GetScheduleOfEmployee(int id)
        {
            var schedule = await _employeeScheduleService.GetScheduleOfEmployee(id);
            if (!schedule.Any()) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "No customers found!",
            });
            return Ok(new ApiResponseModel<List<EmployeeScheduleResponse>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Fetch data successfully!",
                Response = schedule
            });
        }

        //GET: api/Schedule
        [HttpGet("schedules/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeScheduleResponse>> GetSchedule(int id)
        {
            var schedule = await _employeeScheduleService.GetSchedule(id);

            if (schedule == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Schedule not found!"
                });
            }

            return Ok(new ApiResponseModel<EmployeeScheduleResponse>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Fetch data successfully!",
                Response = schedule
            });
        }

        //PUT: api/Schedule
        [HttpPut("schedule/{id}")]
        public async Task<IActionResult> PutSchedule(int id, UpdateEmployeeSchedule employeeSchedule)
        {
            if (id != employeeSchedule.Id)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Id does not match!"
                });
            }
            var result = await _employeeScheduleService.UpdateSchedule(employeeSchedule);
            if (!result)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Failed to update schedule!"
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
