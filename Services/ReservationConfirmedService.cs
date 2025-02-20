using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.Models;

namespace otel_advisor_webApp.Services
{
    public class ReservationConfirmedService
    {
        private readonly HotelContext _context;

        public ReservationConfirmedService(HotelContext context)
        {
            _context = context;
        }

        // Check if a reservation request is already confirmed
        public async Task<bool> CheckIfReservationConfirmed(int reservation_request_id)
        {
            // Check if there is any confirmed reservation for the given reservation request ID
            return await _context.ReservationConfirmed
                                 .AnyAsync(rc => rc.reservation_request_id == reservation_request_id);
        }
    }
}
