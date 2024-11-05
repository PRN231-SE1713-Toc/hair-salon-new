using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Contracts.Services;
using AutoMapper;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Responses;

namespace HairSalon.Api.Controllers.v1
{
    public class AppointmentsController : BaseApiController
    {
        private readonly HairSalonDbContext _context;
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentsController(HairSalonDbContext context, IAppointmentServices appointmentServices)
        {
            _context = context;
            _appointmentServices = appointmentServices;
        }

       /// <summary>
       /// Get appointments
       /// </summary>
       /// <returns></returns>
       [HttpGet("appointments")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AppointmentViewResponse>>> GetAppointments()
        {
            var appointments = await _appointmentServices.GetAppointments();

            if (!appointments.Any())
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No appointments!"
                });

            return Ok(appointments);
        }

        /// <summary>
        /// Get appointment by id
        /// </summary>
        /// <param name="id">Appointment's id</param>
        /// <returns></returns>
        [HttpGet("appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentViewResponse>> GetAppointment(int id)
        {
            var appointment = await _appointmentServices.GetAppointment(id);

            if (appointment == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Appointment not found!"
                });
            }

            return Ok(appointment);
        }

        // TODO: Change the action result of this endpoint, return Ok with the message or NoContent
        [HttpPut("appointment/{id}")]
        public async Task<IActionResult> PutAppointment(AppointmentUpdateModel appointment)
        {
            try
            {
                string mess = await _appointmentServices.UpdateAppointment(appointment);

                if (mess == "Appointment updated successfully")
                {
                    var appointments = await _appointmentServices.GetAppointments();
                    AppointmentViewResponse appointmentView = appointments.LastOrDefault();
                    return CreatedAtAction("GetAppointment", new { id = appointmentView.Id }, appointmentView);
                }
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = mess
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }
        
        // TODO: Change the action result of this endpoint, return Ok with the message or NoContent
        [HttpPost("appointment")]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentCreateModel appointment)
        {
            try
            {
                string mess = await _appointmentServices.CreateAppointment(appointment);

                if (mess == "new appointment added successfully")
                {
                    var appointments = await _appointmentServices.GetAppointments();
                    AppointmentViewResponse appointmentView = appointments.LastOrDefault();
                    return CreatedAtAction("GetAppointment", new { id = appointmentView.Id }, appointmentView);
                }
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = mess
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }
        
        // TODO: Change the action result of this endpoint, return Ok with the message or NoContent
        [HttpDelete("appointment/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                string mess = await _appointmentServices.DeleteAppointment(id);

                if (mess == "Deleted successfully")
                {
                    var appointments = await _appointmentServices.GetAppointments();
                    AppointmentViewResponse appointmentView = appointments.LastOrDefault();
                    return CreatedAtAction("GetAppointment", new { id = appointmentView.Id }, appointmentView);
                }
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = mess
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }
    }
}
