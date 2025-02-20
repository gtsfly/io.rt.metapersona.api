using System.ComponentModel.DataAnnotations;

namespace otel_advisor_webApp.Models
{
    public class ReservationOffer
    {
        [Key]
        public string offer_key { get; set; } // Pk which reservation_request_id+hotel_id
        public DateTime check_in_date { get; set; }
        public DateTime check_out_date { get; set; }
        public decimal price { get; set; }
        public string room_type { get; set; }
        public string board_type { get; set; }
    }
}
