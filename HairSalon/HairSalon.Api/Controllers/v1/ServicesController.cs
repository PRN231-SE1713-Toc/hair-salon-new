using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Infrastructure;
using AutoMapper;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Service;

namespace HairSalon.Api.Controllers.v1
{
    public class ServicesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IServiceService _hairService;

        public ServicesController(IMapper mapper, IServiceService hairService)
        {
            _mapper = mapper;
            _hairService = hairService;
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns></returns>
        [HttpGet("service")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
        {
            var hairService = await _hairService.GetServices();
            if (hairService == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, _mapper.Map<List<ServiceDto>>(hairService));
        }

        /// <summary>
        /// Get hair service by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("service/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hairService = await _hairService.GetServicesById(id);
            if (hairService == null)
            {
                return StatusCode(404);
            }

            return StatusCode(200, _mapper.Map<ServiceDto>(hairService));
        }

        /// <summary>
        /// Update service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> PutService(int id, UpdateServiceRequest service)
        {

            try
            {
                var updatedService = await _hairService.UpdateService(id, service);
                if (updatedService == null)
                {
                    return NotFound(new { error = "Service not found." });
                }

                return Ok(updatedService);
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

        [HttpPost("services")]
        public async Task<IActionResult> Create(CreateServiceModel createServiceModel)
        {
            try
            {
                Core.Entities.Service service = new Core.Entities.Service
                {
                    Name = createServiceModel.Name,
                    Description = createServiceModel.Description,
                    Duration = createServiceModel.Duration,
                    Price = createServiceModel.Price,
                };

                await _hairService.CreateService(service);
                return Ok(service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message});
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
