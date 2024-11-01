﻿using Microsoft.AspNetCore.Mvc;
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
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Appointment not found!"
                });
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
        [HttpDelete("appointment/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentServices.DeleteAppointment(id);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Cannot update appointment. Operation failed!"
                });

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _appointmentServices.GetAppointment(id) != null;
        }
    }
}
