using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.Models;
using otel_advisor_webApp.DTO;
using System;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserHotelExperienceController : ControllerBase
    {
        private readonly HotelContext _context;

        public UserHotelExperienceController(HotelContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserHotelExperienceDto>> PostUserHotelExperience(UserHotelExperienceDto experienceDto)
        {
            var user = await _context.Def_User.FindAsync(experienceDto.user_id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var existingExperience = await _context.UserHotelExperiences
                .FirstOrDefaultAsync(e => e.user_id == experienceDto.user_id && e.hotel_id == experienceDto.hotel_id);

            if (existingExperience != null)
            {
                existingExperience.overall_rating = experienceDto.overall_rating;
                existingExperience.experience_1 = experienceDto.experience_1;
                existingExperience.experience_1_rating = experienceDto.experience_1_rating;
                existingExperience.experience_2 = experienceDto.experience_2;
                existingExperience.experience_2_rating = experienceDto.experience_2_rating;
                existingExperience.experience_3 = experienceDto.experience_3;
                existingExperience.experience_3_rating = experienceDto.experience_3_rating;
                existingExperience.most_liked_experience = experienceDto.most_liked_experience;
                existingExperience.least_liked_experience = experienceDto.least_liked_experience;
                existingExperience.would_visit_again = experienceDto.would_visit_again;
                existingExperience.additional_comments = experienceDto.additional_comments;
                existingExperience.created_at = DateTime.UtcNow; 

                _context.UserHotelExperiences.Update(existingExperience);
                await _context.SaveChangesAsync();

                experienceDto.user_name = user.name;
                experienceDto.user_hotel_experience_id = existingExperience.user_hotel_experience_id;
                experienceDto.created_at = existingExperience.created_at;

                return Ok(experienceDto); 
            }
            else
            {
                var newExperience = new UserHotelExperience
                {
                    user_id = experienceDto.user_id,
                    hotel_id = experienceDto.hotel_id,
                    reservation_request_id = experienceDto.reservation_request_id,
                    overall_rating = experienceDto.overall_rating,
                    experience_1 = experienceDto.experience_1,
                    experience_1_rating = experienceDto.experience_1_rating,
                    experience_2 = experienceDto.experience_2,
                    experience_2_rating = experienceDto.experience_2_rating,
                    experience_3 = experienceDto.experience_3,
                    experience_3_rating = experienceDto.experience_3_rating,
                    most_liked_experience = experienceDto.most_liked_experience,
                    least_liked_experience = experienceDto.least_liked_experience,
                    would_visit_again = experienceDto.would_visit_again,
                    additional_comments = experienceDto.additional_comments,
                    created_at = DateTime.UtcNow
                };

                _context.UserHotelExperiences.Add(newExperience);
                await _context.SaveChangesAsync();

                experienceDto.user_name = user.name;
                experienceDto.user_hotel_experience_id = newExperience.user_hotel_experience_id;
                experienceDto.created_at = newExperience.created_at;

                return CreatedAtAction(nameof(GetUserHotelExperience), new { id = newExperience.user_hotel_experience_id }, experienceDto);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserHotelExperienceDto>> GetUserHotelExperience(int id)
        {
            var experience = await _context.UserHotelExperiences
                                           .Include(e => e.User)  
                                           .FirstOrDefaultAsync(e => e.user_hotel_experience_id == id);

            if (experience == null)
            {
                return NotFound();
            }

            var experienceDto = new UserHotelExperienceDto
            {
                user_hotel_experience_id = experience.user_hotel_experience_id,
                user_id = experience.user_id,
                user_name = experience.User.name,  
                hotel_id = experience.hotel_id,
                reservation_request_id = experience.reservation_request_id,
                overall_rating = experience.overall_rating,
                experience_1 = experience.experience_1,
                experience_1_rating = experience.experience_1_rating,
                experience_2 = experience.experience_2,
                experience_2_rating = experience.experience_2_rating,
                experience_3 = experience.experience_3,
                experience_3_rating = experience.experience_3_rating,
                most_liked_experience = experience.most_liked_experience,
                least_liked_experience = experience.least_liked_experience,
                would_visit_again = experience.would_visit_again,
                additional_comments = experience.additional_comments,
                created_at = experience.created_at
            };

            return experienceDto;
        }
        [HttpGet("feedback-status")]
        public async Task<ActionResult<IEnumerable<object>>> GetFeedbackStatus()
        {
            var reservationsWithFeedbackStatus = await _context.UserHotelExperiences
                .GroupBy(uhe => uhe.reservation_request_id)
                .Select(group => new
                {
                    ReservationId = group.Key,
                    HasFeedback = true
                })
                .ToListAsync();

            return Ok(reservationsWithFeedbackStatus);
        }
    }
}
