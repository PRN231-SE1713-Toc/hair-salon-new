using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalon.Infrastructure;

namespace HairSalon.Api.Controllers.v1
{
    public class ServicesController : BaseApiController
    {
        private readonly HairSalonDbContext _context;

        public ServicesController(HairSalonDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Core.Entities.Service>>> GetServices()
        //{
        //    return await _context.Services.ToListAsync();
        //}

        // GET: api/Services/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Core.Entities.Service>> GetService(int id)
        //{
        //    var service = await _context.Services.FindAsync(id);

        //    if (service == null)
        //    {
        //        return NotFound();
        //    }

        //    return service;
        //}

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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Core.Entities.Service>> PostService(Core.Entities.Service service)
        //{
        //    _context.Services.Add(service);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetService", new { id = service.Id }, service);
        //}

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
