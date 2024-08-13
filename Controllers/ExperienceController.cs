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
    public class ExperienceController : ControllerBase
    {
        private readonly HotelContext _context;

        public ExperienceController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
        {
            var experiences = await _context.Inf_Experience
                .Select(e => new ExperienceDto
                {
                    experience_id = e.experience_id,
                    name = e.name,
                    description = e.description
                })
                .ToListAsync();
            return Ok(experiences);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExperienceDto>> GetExperience(int id)
        {
            var experience = await _context.Inf_Experience
                .Where(e => e.experience_id == id)
                .Select(e => new ExperienceDto
                {
                    experience_id = e.experience_id,
                    name = e.name,
                    description = e.description
                })
                .FirstOrDefaultAsync();

            if (experience == null)
            {
                return NotFound();
            }

            return Ok(experience);
        }

        [HttpPost]
        public async Task<ActionResult<ExperienceDto>> CreateExperience(ExperienceDto experienceDto)
        {
            var experience = new Experience
            {
                name = experienceDto.name,
                description = experienceDto.description
            };

            _context.Inf_Experience.Add(experience);
            await _context.SaveChangesAsync();

            // Experience id to DTO
            experienceDto.experience_id = experience.experience_id;

            return CreatedAtAction(nameof(GetExperience), new { id = experienceDto.experience_id }, experienceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExperience(int id, ExperienceDto experienceDto)
        {
            if (id != experienceDto.experience_id)
            {
                return BadRequest();
            }

            var experience = await _context.Inf_Experience.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            experience.name = experienceDto.name;
            experience.description = experienceDto.description;

            _context.Entry(experience).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _context.Inf_Experience.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            _context.Inf_Experience.Remove(experience);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
