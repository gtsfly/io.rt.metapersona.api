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
    public class HotelController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetDef_Hotel()
        {
            var defHotel = await _context.Def_Hotel
                .Include(h => h.Location)  
                .Select(h => new HotelDto
                {
                    hotel_id = h.hotel_id,
                    name = h.name,
                    location = h.Location.name,  
                    rating = h.rating
                }).ToListAsync();
            return Ok(defHotel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _context.Def_Hotel
                .Include(h => h.Location)  
                .Where(h => h.hotel_id == id)
                .Select(h => new HotelDto
                {
                    hotel_id = h.hotel_id,
                    name = h.name,
                    location = h.Location.name,  
                    rating = h.rating
                }).FirstOrDefaultAsync();

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        // Get locations
        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<string>>> GetLocations()
        {
            var locations = await _context.Def_Location
                .Select(l => l.name)
                .Distinct()
                .ToListAsync();
            return Ok(locations);
        }

        [HttpPost]
        public async Task<ActionResult<HotelDto>> PostHotel([FromBody] HotelDto hotelDto)
        {
            var location = await _context.Def_Location
                .FirstOrDefaultAsync(l => l.name == hotelDto.location);

            if (location == null)
            {
                return BadRequest("Invalid location");
            }

            var hotel = new Hotel
            {
                name = hotelDto.name,
                location_id = location.location_id,
                rating = hotelDto.rating
            };

            _context.Def_Hotel.Add(hotel);
            await _context.SaveChangesAsync();

            hotelDto.hotel_id = hotel.hotel_id;

            return CreatedAtAction("GetHotel", new { id = hotel.hotel_id }, hotelDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, [FromBody] HotelDto hotelDto)
        {
            var hotel = await _context.Def_Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            var location = await _context.Def_Location
                .FirstOrDefaultAsync(l => l.name == hotelDto.location);

            if (location == null)
            {
                return BadRequest("Invalid location");
            }

            hotel.name = hotelDto.name;
            hotel.location_id = location.location_id;
            hotel.rating = hotelDto.rating;

            _context.Entry(hotel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Def_Hotel.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Def_Hotel.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
