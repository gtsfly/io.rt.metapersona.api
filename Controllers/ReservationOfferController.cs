using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.DTOs;
using otel_advisor_webApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace otel_advisor_webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationOfferController : ControllerBase
    {
        private readonly HotelContext _context;

        public ReservationOfferController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationOfferDto>>> GetReservationOffers()
        {
            var reservationOffers = await _context.Inf_ReservationOffer.ToListAsync();
            var reservationOfferDtos = new List<ReservationOfferDto>();

            foreach (var offer in reservationOffers)
            {
                reservationOfferDtos.Add(new ReservationOfferDto
                {
                    offer_key = offer.offer_key,
                    check_in_date = DateTime.SpecifyKind(offer.check_in_date, DateTimeKind.Utc),
                    check_out_date = DateTime.SpecifyKind(offer.check_out_date, DateTimeKind.Utc),
                    price = offer.price,
                    room_type = offer.room_type,
                    board_type = offer.board_type,
                });
            }

            return Ok(reservationOfferDtos);
        }

        [HttpGet("{offer_key}")]
        public async Task<ActionResult<ReservationOfferDto>> GetReservationOffer(string offer_key)
        {
            var offer = await _context.Inf_ReservationOffer.FindAsync(offer_key);

            if (offer == null)
            {
                return NotFound();
            }

            var offerDto = new ReservationOfferDto
            {
                offer_key = offer.offer_key,
                check_in_date = DateTime.SpecifyKind(offer.check_in_date, DateTimeKind.Utc),
                check_out_date = DateTime.SpecifyKind(offer.check_out_date, DateTimeKind.Utc),
                price = offer.price,
                room_type = offer.room_type,
                board_type = offer.board_type,
            };

            return Ok(offerDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationOfferDto>> PostReservationOffer(ReservationOfferDto offerDto)
        {
            var offer = new ReservationOffer
            {
                offer_key = offerDto.offer_key,
                check_in_date = DateTime.SpecifyKind(offerDto.check_in_date, DateTimeKind.Utc),
                check_out_date = DateTime.SpecifyKind(offerDto.check_out_date, DateTimeKind.Utc),
                price = offerDto.price,
                room_type = offerDto.room_type,
                board_type=offerDto.board_type,
            };

            _context.Inf_ReservationOffer.Add(offer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationOffer), new { offer_key = offer.offer_key }, offerDto);
        }

        [HttpPut("{offer_key}")]
        public async Task<IActionResult> PutReservationOffer(string offer_key, ReservationOfferDto offerDto)
        {
            if (offer_key != offerDto.offer_key)
            {
                return BadRequest();
            }

            var offer = await _context.Inf_ReservationOffer.FindAsync(offer_key);
            if (offer == null)
            {
                return NotFound();
            }

            offer.check_in_date = offerDto.check_in_date;
            offer.check_out_date = offerDto.check_out_date;
            offer.price = offerDto.price;
            offer.room_type = offerDto.room_type;
            offer.board_type = offerDto.board_type;

            _context.Entry(offer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationOfferExists(offer_key))
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

        [HttpDelete("{offer_key}")]
        public async Task<IActionResult> DeleteReservationOffer(string offer_key)
        {
            var offer = await _context.Inf_ReservationOffer.FindAsync(offer_key);
            if (offer == null)
            {
                return NotFound();
            }

            _context.Inf_ReservationOffer.Remove(offer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationOfferExists(string offer_key)
        {
            return _context.Inf_ReservationOffer.Any(e => e.offer_key == offer_key);
        }
    }
}
