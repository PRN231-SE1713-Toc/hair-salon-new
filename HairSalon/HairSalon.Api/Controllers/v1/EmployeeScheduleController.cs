using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeScheduleResponse>>> GetSchedule()
        {
            var schedule = await _employeeScheduleService.GetSchedule();

            if (schedule.Response == null || !schedule.Response.Any())
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No customers found!"
                });
            }

            return Ok(new ApiResponseModel<List<EmployeeScheduleResponse>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Schedules retrieved successfully.",
                Response = schedule.Response
            });

        }

        //GET: api/employee/{id}/schedules
        [HttpGet("employee/{id}/schedules")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseModel<List<EmployeeScheduleResponse>>>> GetScheduleOfEmployee(int id)
        {
            var schedule = await _employeeScheduleService.GetScheduleOfEmployee(id);

            if (schedule.Response == null || !schedule.Response.Any())
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No schedules found for employee with ID {id}!",
                    Response = null
                });
            }

            return Ok(new ApiResponseModel<List<EmployeeScheduleResponse>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Schedules retrieved successfully.",
                Response = schedule.Response
            });
        }


        //GET: api/Schedule/{id}
        [HttpGet("schedules/{id}")]
        [Authorize]
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

        // UPDATE
        [HttpPut("schedule/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> UpdateSchedule(int id, UpdateEmployeeSchedule employeeSchedule)
        {
            var updatedSchedule = await _employeeScheduleService.UpdateSchedule(id, employeeSchedule);

            if (updatedSchedule == null)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Failed to update schedule!"
                });
            }

            return Ok(updatedSchedule);
        }

        // CREATE
        [HttpPost("schedules")]
        [Authorize]
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

            return CreatedAtAction(nameof(GetSchedule), new { id = createdSchedule.Response?.Id }, new ApiResponseModel<EmployeeScheduleResponse>
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Schedule created successfully",
                Response = createdSchedule.Response
            });
        }

        //DELETE
        [HttpDelete("schedule/{id}")]
        [Authorize]  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ApiResponseModel<string>))]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _employeeScheduleService.GetScheduleEntityById(id);

            if (schedule == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Schedule not found!",
                    Response = null
                });
            }

            var result = await _employeeScheduleService.DeleteSchedule(schedule);

            if (!result.Response)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to delete schedule!",
                    Response = null
                });
            }

            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Schedule deleted successfully.",
                Response = "Success"
            });
        }

        /// <summary>
        /// Get available stylists for a given date and time range
        /// </summary>
        /// <param name="appointmentDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet("date/{appointmentDate}/stylists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvailableStylists(DateOnly appointmentDate, TimeOnly? startTime, TimeOnly? endTime)
        {
            var stylists = await _employeeScheduleService.GetAvailableStylists(appointmentDate, startTime, endTime);
            if (!stylists.Any())
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No available stylists found!"
                });
            }
            return Ok(stylists);
        }
    }
}
