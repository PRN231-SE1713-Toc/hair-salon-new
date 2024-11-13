using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;

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
       [Authorize]
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
        [Authorize]
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
                    Message = "Appointments not found!"
                });
            }

            return Ok(appointment);
        }

        [HttpGet("appointments/customerId/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentViewResponse>> GetAppointmentbyCustomerId(int id, int status)
        {
            //var customer
            var appointment = await _appointmentServices.GetAppointmentsbyCustomerId(id, status);

            if (appointment == null || appointment.Count == 0)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Appointments not found!"
                });
            }

            return Ok(appointment);
        }

        [HttpGet("appointments/stylistId/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AppointmentViewResponse>> GetAppointmentbyStylistId(int id, int status)
        {
            var appointment = await _appointmentServices.GetAppointmentsByStylistId(id, status);

            if (appointment == null || appointment.Count == 0)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Appointments not found!"
                });
            }

            return Ok(appointment);
        }

        //PUT: api/Appointments/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("appointment/{id}")]
        [Authorize]
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

        //POST: api/Appointments
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("appointment")]
        [Authorize]
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

        //DELETE: api/Appointments/5
        [HttpDelete("appointment/{id}")]
        [Authorize]
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
