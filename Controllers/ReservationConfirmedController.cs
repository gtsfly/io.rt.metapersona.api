using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTOs;
using otel_advisor_webApp.Models;
using otel_advisor_webApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationConfirmedController : ControllerBase
    {
        private readonly HotelContext _context;
        private readonly ReservationConfirmedService _reservationConfirmedService;

        public ReservationConfirmedController(HotelContext context, ReservationConfirmedService reservationConfirmedService)
        {
            _context = context;
            _reservationConfirmedService = reservationConfirmedService;
        }

        // GET: api/ReservationConfirmed
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationConfirmed>>> GetAllConfirmedReservations()
        {
            return await _context.ReservationConfirmed.ToListAsync();
        }

        // GET: api/ReservationConfirmed/{reservation_request_id}
        [HttpGet("{reservation_request_id}")]
        public async Task<IActionResult> CheckIfReservationConfirmed(int reservation_request_id)
        {
            var isConfirmed = await _reservationConfirmedService.CheckIfReservationConfirmed(reservation_request_id);
            return Ok(new { isConfirmed = isConfirmed });
        }

        // POST: api/ReservationConfirmed
        [HttpPost]
        public async Task<ActionResult<ReservationConfirmed>> ConfirmReservation([FromBody] ReservationConfirmedDto reservationConfirmedDto)
        {
            try
            {
                var reservationRequest = await _context.Inf_Reservation.FindAsync(reservationConfirmedDto.reservation_request_id);
                var hotel = await _context.Def_Hotel.FindAsync(reservationConfirmedDto.hotel_id);
                var user = await _context.Def_User.FindAsync(reservationRequest.user_id);

                if (reservationRequest == null || hotel == null)
                {
                    return NotFound("Rezervasyon talebi veya otel bulunamadı.");
                }

                var reservationConfirmed = new ReservationConfirmed
                {
                    reservation_request_id = reservationConfirmedDto.reservation_request_id,
                    hotel_id = reservationConfirmedDto.hotel_id,
                    check_in_date = DateTime.SpecifyKind(reservationConfirmedDto.check_in_date, DateTimeKind.Utc),
                    check_out_date = DateTime.SpecifyKind(reservationConfirmedDto.check_out_date, DateTimeKind.Utc),
                    price = reservationConfirmedDto.price,
                    room_type = reservationConfirmedDto.room_type,
                    board_type = reservationConfirmedDto.board_type,
                    confirm_date = DateTime.UtcNow,
                    user_id = user.user_id
                };

                _context.ReservationConfirmed.Add(reservationConfirmed);
                await _context.SaveChangesAsync();

                reservationConfirmedDto.confirmed_reservation_id = reservationConfirmed.confirmed_reservation_id;
                reservationConfirmedDto.user_id = user.user_id;
                reservationConfirmedDto.user_name = user.name;

                return CreatedAtAction("GetReservationConfirmed", new { confirmed_reservation_id = reservationConfirmed.confirmed_reservation_id }, reservationConfirmedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return StatusCode(500, "Rezervasyon onaylanırken bir hata oluştu.");
            }
        }

        // GET: api/ReservationConfirmed/check/{confirmed_reservation_id}
        [HttpGet("check/{confirmed_reservation_id}")]
        public async Task<ActionResult<ReservationConfirmedDto>> GetReservationConfirmed(int confirmed_reservation_id)
        {
            var reservationConfirmed = await _context.ReservationConfirmed
                                                      .Include(r => r.User) 
                                                      .FirstOrDefaultAsync(r => r.confirmed_reservation_id == confirmed_reservation_id);

            if (reservationConfirmed == null)
            {
                return NotFound("Rezervasyon onayı bulunamadı.");
            }

            var reservationConfirmedDto = new ReservationConfirmedDto
            {
                confirmed_reservation_id = reservationConfirmed.confirmed_reservation_id,
                reservation_request_id = reservationConfirmed.reservation_request_id,
                hotel_id = reservationConfirmed.hotel_id,
                check_in_date = reservationConfirmed.check_in_date,
                check_out_date = reservationConfirmed.check_out_date,
                price = reservationConfirmed.price,
                room_type = reservationConfirmed.room_type,
                board_type = reservationConfirmed.board_type,
                confirm_date = reservationConfirmed.confirm_date,
                user_id = reservationConfirmed.user_id,
                user_name = reservationConfirmed.User.name  
            };

            return Ok(reservationConfirmedDto);
        }
    }
}