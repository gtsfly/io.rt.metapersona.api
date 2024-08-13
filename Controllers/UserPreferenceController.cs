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
    public class UserPreferenceController : ControllerBase
    {
        private readonly HotelContext _context;

        public UserPreferenceController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPreferenceDto>>> GetUserPreferences()
        {
            var userPreferences = await _context.Rel_UserPreference
                .Select(up => new UserPreferenceDto
                {
                    user_id = up.user_id,
                    experience_id = up.experience_id,
                    priority = up.priority
                }).ToListAsync();
            return Ok(userPreferences);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserPreferenceDto>> GetUserPreference(int id)
        {
            var userPreference = await _context.Rel_UserPreference
                .Where(up => up.user_preference_id == id)
                .Select(up => new UserPreferenceDto
                {
                    user_id = up.user_id,
                    experience_id = up.experience_id,
                    priority = up.priority
                }).FirstOrDefaultAsync();

            if (userPreference == null)
            {
                return NotFound();
            }
            return userPreference;
        }

        [HttpPost]
        public async Task<ActionResult<UserPreferenceDto>> PostUserPreference([FromBody] UserPreferenceDto dto)
        {
            var userPreference = new UserPreference
            {
                user_id = dto.user_id,
                experience_id = dto.experience_id,
                priority = dto.priority
            };

            _context.Rel_UserPreference.Add(userPreference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPreference", new { id = userPreference.user_preference_id }, new UserPreferenceDto
            {
                user_id = userPreference.user_id,
                experience_id = userPreference.experience_id,
                priority = userPreference.priority
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserPreference(int id, [FromBody] UserPreferenceDto dto)
        {
            var userPreference = await _context.Rel_UserPreference.FindAsync(id);
            if (userPreference == null)
            {
                return NotFound();
            }

            userPreference.user_id = dto.user_id;
            userPreference.experience_id = dto.experience_id;
            userPreference.priority = dto.priority;

            _context.Entry(userPreference).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPreference(int id)
        {
            var userPreference = await _context.Rel_UserPreference.FindAsync(id);
            if (userPreference == null)
            {
                return NotFound();
            }

            _context.Rel_UserPreference.Remove(userPreference);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
