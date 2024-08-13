using Microsoft.AspNetCore.Mvc;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTO;
using otel_advisor_webApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelExperienceController : ControllerBase
    {
        private readonly HotelContext _context;

        public HotelExperienceController(HotelContext context)
        {
            _context = context;
        }

        // Get all Rel_HotelExperience with projection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelExperienceDto>>> GetRel_HotelExperience()
        {
            var relHotelExperience = await _context.Rel_HotelExperience
                .Select(he => new HotelExperienceDto
                {
                    hotel_id = he.hotel_id,
                    experience_id = he.experience_id,
                    rating = he.rating
                }).ToListAsync();
            return Ok(relHotelExperience);
        }

        // Get a single HotelExperience by composite key with projection
        [HttpGet("{hotelId}/{experienceId}")]
        public async Task<ActionResult<HotelExperienceDto>> GetHotelExperience(int hotelId, int experienceId)
        {
            var hotelExperience = await _context.Rel_HotelExperience
                .Where(he => he.hotel_id == hotelId && he.experience_id == experienceId)
                .Select(he => new HotelExperienceDto
                {
                    hotel_id = he.hotel_id,
                    experience_id = he.experience_id,
                    rating = he.rating
                }).FirstOrDefaultAsync();

            if (hotelExperience == null)
            {
                return NotFound();
            }

            return Ok(hotelExperience);
        }

        // Create a new HotelExperience
        [HttpPost]
        public async Task<ActionResult<HotelExperienceDto>> PostHotelExperience([FromBody] HotelExperienceDto hotelExperienceDto)
        {
            var hotelExperience = new HotelExperience
            {
                hotel_id = hotelExperienceDto.hotel_id,
                experience_id = hotelExperienceDto.experience_id,
                rating = hotelExperienceDto.rating
            };

            _context.Rel_HotelExperience.Add(hotelExperience);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HotelExperienceExists(hotelExperienceDto.hotel_id, hotelExperienceDto.experience_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHotelExperience", new { hotelId = hotelExperience.hotel_id, experienceId = hotelExperience.experience_id }, hotelExperienceDto);
        }

        // Update an existing HotelExperience
        [HttpPut("{hotelId}/{experienceId}")]
        public async Task<IActionResult> PutHotelExperience(int hotelId, int experienceId, [FromBody] HotelExperienceDto hotelExperienceDto)
        {
            if (hotelId != hotelExperienceDto.hotel_id || experienceId != hotelExperienceDto.experience_id)
            {
                return BadRequest();
            }

            var hotelExperience = await _context.Rel_HotelExperience
                .Where(he => he.hotel_id == hotelId && he.experience_id == experienceId)
                .FirstOrDefaultAsync();

            if (hotelExperience == null)
            {
                return NotFound();
            }

            hotelExperience.rating = hotelExperienceDto.rating; // set updated rating
            _context.Entry(hotelExperience).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExperienceExists(hotelId, experienceId))
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

        // Delete a HotelExperience
        [HttpDelete("{hotelId}/{experienceId}")]
        public async Task<IActionResult> DeleteHotelExperience(int hotelId, int experienceId)
        {
            var hotelExperience = await _context.Rel_HotelExperience
                .Where(he => he.hotel_id == hotelId && he.experience_id == experienceId)
                .FirstOrDefaultAsync();

            if (hotelExperience == null)
            {
                return NotFound();
            }

            _context.Rel_HotelExperience.Remove(hotelExperience);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExperienceExists(int hotelId, int experienceId)
        {
            return _context.Rel_HotelExperience.Any(e => e.hotel_id == hotelId && e.experience_id == experienceId);
        }
    }
}
