using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HairSalon.Api.Controllers.v1
{
    public class EmployeeScheduleController : BaseApiController
    {
        private readonly IEmployeeScheduleService _employeeScheduleService;

        public EmployeeScheduleController(IEmployeeScheduleService employeeScheduleService)
        {
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
            return Ok(schedule);
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
            return Ok(schedule);
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

            return Ok(schedule);
        }

        
        [HttpPut("schedule/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> UpdateSchedule(int id, UpdateEmployeeSchedule employeeSchedule)
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
            return NoContent();
        }

        [HttpPost("schedules")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateEmployeeScheduleModel createScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid data provided",
                    Response = null
                });
            }

            var createdSchedule = await _employeeScheduleService.CreateSchedule(createScheduleDto);

            if (createdSchedule == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error creating schedule",
                    Response = null
                });
            }

            return CreatedAtAction(nameof(GetSchedule), new { id = createdSchedule.Id }, new ApiResponseModel<EmployeeScheduleResponse>
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Schedule created successfully",
                Response = createdSchedule
            });
        }
    }
}
