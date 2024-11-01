using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;
using HairSalon.Infrastructure;
using HairSalon.Core.Contracts.Services;
using AutoMapper;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Dtos.Requests;

namespace HairSalon.Api.Controllers.v1
{
    public class AppointmentsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentsController(IMapper mapper, IAppointmentServices appointmentServices)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;
        }

       //GET: api/Appointments
       [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentViewResponse>>> GetAppointments()
        {
            var appointments = await _appointmentServices.GetAppointments();
            if (appointments == null)
            {
                return NotFound();
            } else
            {
                return Ok(appointments);
            }
        }

        //GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentViewResponse>> GetAppointment(int id)
        {
            var appointment = await _appointmentServices.GetAppointment(id);

            if (appointment == null)
            {
                return NotFound(id);
            }

            return Ok(appointment);
        }

        //PUT: api/Appointments/5
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, AppointmentUpdateModel appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            try
            {
                await _appointmentServices.UpdateAppointment(appointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/Appointments
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentCreateModel appointment)
        {
            try
            {
                await _appointmentServices.CreateAppointment(appointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
            var appointments = await _appointmentServices.GetAppointments();
            AppointmentViewResponse appointmentView =appointments.LastOrDefault();
            return CreatedAtAction("GetAppointment", new { id = appointmentView.Id }, appointmentView);
        }

        //DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentServices.GetAppointment(id);
            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                await _appointmentServices.DeleteAppointment(id);
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _appointmentServices.GetAppointment(id) != null;
        }
    }
}
