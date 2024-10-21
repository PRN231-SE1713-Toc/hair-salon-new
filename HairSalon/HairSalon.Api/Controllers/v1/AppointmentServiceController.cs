using AutoMapper;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Api.Controllers.v1
{
    public class ApppointmentServiceController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServiceService _hairService;

        public ApppointmentServiceController(IMapper mapper, IAppointmentServiceService hairService)
        {
            _mapper = mapper;
            _hairService = hairService;
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns></returns>
        [HttpGet("appointment-service")]
        public async Task<ActionResult<IEnumerable<AppointmentServiceDto>>> GetServices()
        {
            var hairService = await _hairService.GetAllServicesAsync();
            if (hairService == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, _mapper.Map<List<AppointmentServiceDto>>(hairService));
        }

        /// <summary>
        /// Get hair service by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("appointment-service/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hairService = await _hairService.GetServiceByIdAsync(id);
            if (hairService == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, _mapper.Map<AppointmentServiceDto>(hairService));
        }

        /// <summary>
        /// Update service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPut("appointment-service/{id}")]
        public async Task<ActionResult<AppointmentService>> Update(int id, [FromBody] UpdateAppointmentServiceModel model)
        {
            try
            {
                var service = await _hairService.UpdateServiceAsync(id, model);
                if (service == null) return NotFound(new { error = "Service not found." });
                return Ok(service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("appointment-service/")]
        public async Task<IActionResult> Create(CreateAppointmentServiceModel createServiceModel)
        {
            try
            {
                var service = await _hairService.CreateServiceAsync(createServiceModel);
                return Ok(service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Services/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteService(int id)
        //{
        //    var service = await _context.Services.FindAsync(id);
        //    if (service == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Services.Remove(service);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}

