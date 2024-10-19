using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Infrastructure;
using AutoMapper;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Dtos.Requests;

namespace HairSalon.Api.Controllers.v1
{
    public class ServicesController : BaseApiController
    {
        private readonly HairSalonDbContext _context;
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


        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutService(int id, Core.Entities.Service service)
        //{
        //    if (id != service.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(service).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ServiceExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPost("services")]
        public ActionResult Create(CreateServiceModel createServiceModel)
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

                _hairService.CreateService(service);
                return Ok(service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the service." });
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

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
