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
                    reservation_request_id = r.reservation_request_id,
                    user_id = r.user_id,
                    user_name = r.user != null ? r.user.name : null, // user_name is optional to print on admin panel
                    check_in_range_start = r.check_in_range_start,
                    check_in_range_end = r.check_in_range_end,
                    budget = r.budget,
                    location = r.location,
                    stay_duration = r.stay_duration,
                    exp_1 = r.exp_1,
                    exp_1_rating = r.exp_1_rating,
                    exp_2 = r.exp_2,
                    exp_2_rating = r.exp_2_rating,
                    exp_3 = r.exp_3,
                    exp_3_rating = r.exp_3_rating,
                    adult_num = r.adult_num,
                    child_num = r.child_num,
                    children_ages = r.children_ages, 
                    created_at = r.created_at 
                }).ToListAsync();
            return Ok(Inf_Reservation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationRequestDto>> GetReservation(int id)
        {
            var reservation = await _context.Inf_Reservation
                .Include(r => r.user)
                .Where(r => r.reservation_request_id == id)
                .Select(r => new ReservationRequestDto
                {
                    reservation_request_id = r.reservation_request_id,
                    user_id = r.user_id,
                    user_name = r.user != null ? r.user.name : null,
                    check_in_range_start = r.check_in_range_start,
                    check_in_range_end = r.check_in_range_end,
                    budget = r.budget,
                    location = r.location,
                    stay_duration = r.stay_duration,
                    exp_1 = r.exp_1,
                    exp_1_rating = r.exp_1_rating,
                    exp_2 = r.exp_2,
                    exp_2_rating = r.exp_2_rating,
                    exp_3 = r.exp_3,
                    exp_3_rating = r.exp_3_rating,
                    adult_num = r.adult_num,
                    child_num = r.child_num,
                    children_ages = r.children_ages, 
                    created_at = r.created_at 
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
                    check_in_range_start = DateTime.SpecifyKind(reservationDto.check_in_range_start, DateTimeKind.Utc),
                    check_in_range_end = DateTime.SpecifyKind(reservationDto.check_in_range_end, DateTimeKind.Utc),
                    budget = reservationDto.budget,
                    location = reservationDto.location,
                    stay_duration = reservationDto.stay_duration,
                    exp_1 = reservationDto.exp_1,
                    exp_1_rating = reservationDto.exp_1_rating,
                    exp_2 = reservationDto.exp_2,
                    exp_2_rating = reservationDto.exp_2_rating,
                    exp_3 = reservationDto.exp_3,
                    exp_3_rating = reservationDto.exp_3_rating,
                    adult_num = reservationDto.adult_num,
                    child_num = reservationDto.child_num,
                    children_ages = reservationDto.children_ages, 
                    created_at = DateTime.UtcNow 
                };

                _context.Inf_Reservation.Add(reservation);
                await _context.SaveChangesAsync();

                reservationDto.reservation_request_id = reservation.reservation_request_id;
                reservationDto.created_at = reservation.created_at; 

                return CreatedAtAction("GetReservation", new { id = reservation.reservation_request_id }, reservationDto);
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
            reservation.check_in_range_start = reservationDto.check_in_range_start;
            reservation.check_in_range_end = reservationDto.check_in_range_end;
            reservation.budget = reservationDto.budget;
            reservation.location = reservationDto.location;
            reservation.stay_duration = reservationDto.stay_duration;
            reservation.exp_1 = reservationDto.exp_1;
            reservation.exp_1_rating = reservationDto.exp_1_rating;
            reservation.exp_2 = reservationDto.exp_2;
            reservation.exp_2_rating = reservationDto.exp_2_rating;
            reservation.exp_3 = reservationDto.exp_3;
            reservation.exp_3_rating = reservationDto.exp_3_rating;
            reservation.adult_num = reservationDto.adult_num;
            reservation.child_num = reservationDto.child_num;
            reservation.children_ages = reservationDto.children_ages; 
            reservation.created_at = reservationDto.created_at; 

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
