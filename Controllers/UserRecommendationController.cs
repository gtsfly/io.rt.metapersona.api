using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTO;
using otel_advisor_webApp.Models;
using otel_advisor_webApp.Services;
using System;
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
        private readonly RecommendationService _recommendationService;
        private readonly ILogger<UserRecommendationController> _logger;

        public UserRecommendationController(
            HotelContext context, 
            RecommendationService recommendationService,
            ILogger<UserRecommendationController> logger)
        {
            _context = context;
            _recommendationService = recommendationService;
            _logger = logger;
        }

        [HttpPost("recommend/{reservationRequestId}")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetRecommendedHotels(int reservationRequestId)
        {
            try
            {
                _logger.LogInformation($"Hotel recommendations requested for reservation ID {reservationRequestId}.");

                // Get reservation details
                var reservation = await _context.Inf_Reservation
                    .Where(r => r.reservation_request_id == reservationRequestId)
                    .FirstOrDefaultAsync();

                if (reservation == null)
                {
                    _logger.LogWarning($"Reservation ID {reservationRequestId} not found.");
                    return NotFound("Specified reservation request not found.");
                }

                _logger.LogInformation($"Reservation found: User ID {reservation.user_id}, Location: {reservation.location}");

                // Get user's selected experiences
                var experiences = await _context.Inf_Experience
                    .Where(e => e.name == reservation.exp_1 || e.name == reservation.exp_2 || e.name == reservation.exp_3)
                    .ToListAsync();

                if (!experiences.Any())
                {
                    _logger.LogWarning($"No experiences found for reservation ID {reservationRequestId}.");
                    return NotFound("User's preferred experiences not found.");
                }

                _logger.LogInformation($"Number of experiences found: {experiences.Count}");

                // Match experiences with their importance levels
                var experiencePreferences = new List<ExperiencePreference>();
                foreach (var exp in experiences)
                {
                    int importance = 0;
                    if (exp.name == reservation.exp_1) importance = reservation.exp_1_rating;
                    else if (exp.name == reservation.exp_2) importance = reservation.exp_2_rating;
                    else if (exp.name == reservation.exp_3) importance = reservation.exp_3_rating;

                    experiencePreferences.Add(new ExperiencePreference
                    {
                        experience_id = exp.experience_id,
                        importance = importance
                    });

                    _logger.LogInformation($"Experience added: ID {exp.experience_id}, Name: {exp.name}, Importance: {importance}");
                }

                // Get user information
                var user = await _context.Def_User
                    .FirstOrDefaultAsync(u => u.user_id == reservation.user_id);

                if (user == null)
                {
                    _logger.LogWarning($"User ID {reservation.user_id} not found.");
                    return NotFound("User not found.");
                }

                _logger.LogInformation($"User found: ID {user.user_id}, Email: {user.email}");

                // Get location ID
                var location = await _context.Def_Location
                    .FirstOrDefaultAsync(l => l.name == reservation.location);

                if (location == null)
                {
                    _logger.LogWarning($"Location '{reservation.location}' not found.");
                    return NotFound("Location not found.");
                }

                _logger.LogInformation($"Location found: ID {location.location_id}, Name: {location.name}");

                // Create request for Python API
                var request = new RecommendationRequestDto
                {
                    experience_preferences = experiencePreferences,
                    user_email = user.email,
                    location_id = location.location_id
                };

                _logger.LogInformation("Sending request to Python API...");

                try
                {
                    // Get recommendations from Python API
                    var recommendations = await _recommendationService.GetRecommendations(request);
                    _logger.LogInformation($"Successfully received {recommendations.Count} hotel recommendations.");
                    return Ok(recommendations);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while getting recommendations from Python API: {ex.Message}");
                    return StatusCode(500, $"Error while getting hotel recommendations: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred during the process.");
            }
        }
    }
}
