using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTOs;
using otel_advisor_webApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationRequestController : ControllerBase
    {
        private readonly HotelContext _context;

        public ReservationRequestController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationRequestDto>>> GetInf_Reservation()
        {
            var Inf_Reservation = await _context.Inf_Reservation
                .Include(r => r.user) // User details
                .Select(r => new ReservationRequestDto
                {
                    reservation_id = r.reservation_id,
                    user_id = r.user_id,
                    user_name = r.user != null ? r.user.name : null, // user_name is optional to print on admin panel
                    trip_start = r.trip_start,
                    trip_end = r.trip_end,
                    budget = r.budget,
                    location = r.location,
                    stay_duration = r.stay_duration,
                    exp_1 = r.exp_1,
                    exp_1_rating = r.exp_1_rating,
                    exp_2 = r.exp_2,
                    exp_2_rating = r.exp_2_rating,
                    exp_3 = r.exp_3,
                    exp_3_rating = r.exp_3_rating
                }).ToListAsync();
            return Ok(Inf_Reservation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationRequestDto>> GetReservation(int id)
        {
            var reservation = await _context.Inf_Reservation
                .Include(r => r.user) 
                .Where(r => r.reservation_id == id)
                .Select(r => new ReservationRequestDto
                {
                    reservation_id = r.reservation_id,
                    user_id = r.user_id,
                    user_name = r.user != null ? r.user.name : null, 
                    trip_start = r.trip_start,
                    trip_end = r.trip_end,
                    budget = r.budget,
                    location = r.location,
                    stay_duration = r.stay_duration,
                    exp_1 = r.exp_1,
                    exp_1_rating = r.exp_1_rating,
                    exp_2 = r.exp_2,
                    exp_2_rating = r.exp_2_rating,
                    exp_3 = r.exp_3,
                    exp_3_rating = r.exp_3_rating
                }).FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        [HttpPost]
        public async Task<ActionResult<ReservationRequestDto>> PostReservation([FromBody] ReservationRequestDto reservationDto)
        {
            try
            {
                var reservation = new ReservationRequest
                {
                    user_id = reservationDto.user_id,
                    trip_start = DateTime.SpecifyKind(reservationDto.trip_start, DateTimeKind.Utc),
                    trip_end = DateTime.SpecifyKind(reservationDto.trip_end, DateTimeKind.Utc),
                    budget = reservationDto.budget,
                    location = reservationDto.location,
                    stay_duration = reservationDto.stay_duration,
                    exp_1 = reservationDto.exp_1,
                    exp_1_rating = reservationDto.exp_1_rating,
                    exp_2 = reservationDto.exp_2,
                    exp_2_rating = reservationDto.exp_2_rating,
                    exp_3 = reservationDto.exp_3,
                    exp_3_rating = reservationDto.exp_3_rating
                };

                _context.Inf_Reservation.Add(reservation);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReservation", new { id = reservation.reservation_id }, reservationDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, [FromBody] ReservationRequestDto reservationDto)
        {
            var reservation = await _context.Inf_Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.user_id = reservationDto.user_id;
            reservation.trip_start = reservationDto.trip_start;
            reservation.trip_end = reservationDto.trip_end;
            reservation.budget = reservationDto.budget;
            reservation.location = reservationDto.location;
            reservation.stay_duration = reservationDto.stay_duration;
            reservation.exp_1 = reservationDto.exp_1;
            reservation.exp_1_rating = reservationDto.exp_1_rating;
            reservation.exp_2 = reservationDto.exp_2;
            reservation.exp_2_rating = reservationDto.exp_2_rating;
            reservation.exp_3 = reservationDto.exp_3;
            reservation.exp_3_rating = reservationDto.exp_3_rating;

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Inf_Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Inf_Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
