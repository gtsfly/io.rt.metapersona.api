using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTO;
using otel_advisor_webApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly HotelContext _context;

        public LocationController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            var locations = await _context.Def_Location
                .Select(l => new LocationDto
                {
                    location_id = l.location_id,
                    name = l.name
                })
                .ToListAsync();

            return Ok(locations);
        }

        // GET: api/location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            var location = await _context.Def_Location
                .Where(l => l.location_id == id)
                .Select(l => new LocationDto
                {
                    location_id = l.location_id,
                    name = l.name
                })
                .FirstOrDefaultAsync();

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // POST: api/location
        [HttpPost]
        public async Task<ActionResult<LocationDto>> CreateLocation(LocationDto locationDto)
        {
            var location = new Location
            {
                name = locationDto.name
            };

            _context.Def_Location.Add(location);
            await _context.SaveChangesAsync();

            locationDto.location_id = location.location_id;

            return CreatedAtAction(nameof(GetLocation), new { id = location.location_id }, locationDto);
        }

        // PUT: api/location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationDto locationDto)
        {
            if (id != locationDto.location_id)
            {
                return BadRequest();
            }

            var location = await _context.Def_Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            location.name = locationDto.name;

            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/location/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Def_Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Def_Location.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
