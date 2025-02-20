using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTO;
using otel_advisor_webApp.DTOs;
using otel_advisor_webApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRecommendationController : ControllerBase
    {
        private readonly HotelContext _context;

        public UserRecommendationController(HotelContext context)
        {
            _context = context;
        }

        [HttpPost("recommend/{reservationRequestId}")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetRecommendedHotels(int reservationRequestId)
        {
            // Fetch the reservation details using reservation_request_id
            var reservation = await _context.Inf_Reservation
                                             .Where(r => r.reservation_request_id == reservationRequestId)
                                             .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound("No reservation found for the given reservation request.");
            }

            // Fetch the experience IDs for the user's selected experiences
            var experienceIds = await _context.Inf_Experience
                                              .Where(e => e.name == reservation.exp_1 || e.name == reservation.exp_2 || e.name == reservation.exp_3)
                                              .Select(e => e.experience_id)
                                              .ToListAsync();

            if (!experienceIds.Any())
            {
                return NotFound("No experiences found matching the user's preferences.");
            }

            // Fetch hotel experiences matching the user's preferences
            var hotelExperiences = await _context.Rel_HotelExperience
                                                 .Where(he => experienceIds.Contains(he.experience_id))
                                                 .ToListAsync();

            if (!hotelExperiences.Any())
            {
                return NotFound("No hotel experiences found matching the user's preferences.");
            }

            // Calculate the weighted rating for each hotel based on user's preferences
            var hotelRatings = hotelExperiences.GroupBy(he => he.hotel_id)
                .Select(g => new
                {
                    HotelId = g.Key,
                    UserExperienceRating = g.Sum(he =>
                        (experienceIds.IndexOf(he.experience_id) == 0 ? reservation.exp_1_rating * he.rating :
                         experienceIds.IndexOf(he.experience_id) == 1 ? reservation.exp_2_rating * he.rating :
                         reservation.exp_3_rating * he.rating)
                    )
                })
                .ToList();

            // Fetch overall hotel ratings and their locations
            var overallHotelRatings = await _context.Def_Hotel
                                                    .Include(h => h.Location)
                                                    .Where(h => hotelRatings.Select(hr => hr.HotelId).Contains(h.hotel_id))
                                                    .Select(h => new { h.hotel_id, h.rating, h.location_id })
                                                    .ToListAsync();

            // Combine the user experience rating and the overall hotel rating
            var finalHotelRatings = hotelRatings
                .Join(overallHotelRatings, hr => hr.HotelId, ohr => ohr.hotel_id, (hr, ohr) => new
                {
                    hr.HotelId,
                    FinalRating = ((hr.UserExperienceRating / 15) * 0.7) + (ohr.rating * 0.3),
                    LocationId = ohr.location_id
                })
                .OrderByDescending(h => h.FinalRating)
                .ToList();

            // Fetch the location name based on the location ID
            var location = await _context.Def_Location.FirstOrDefaultAsync(l => l.name == reservation.location);

            if (location == null)
            {
                return NotFound("No location found with the specified name.");
            }

            var filteredHotels = finalHotelRatings
                .Where(h => h.LocationId == location.location_id)
                .ToList();

            if (!filteredHotels.Any())
            {
                return NotFound("No hotels found in the specified location.");
            }

            // Convert to DTO and return the result
            var result = filteredHotels.Select(h => new HotelDto
            {
                hotel_id = h.HotelId,
                name = _context.Def_Hotel.Where(hotel => hotel.hotel_id == h.HotelId).Select(hotel => hotel.name).FirstOrDefault() ?? "",
                location = location.name,
                rating = h.FinalRating
            }).ToList();

            return Ok(result);
        }
    }
}
