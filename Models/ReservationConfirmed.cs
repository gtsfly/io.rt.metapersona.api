using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace otel_advisor_webApp.Models
{
    public class ReservationConfirmed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int confirmed_reservation_id { get; set; }

        public int reservation_request_id { get; set; }
        public int hotel_id { get; set; }
        public DateTime check_in_date { get; set; }
        public DateTime check_out_date { get; set; }
        public int price { get; set; }
        public string room_type { get; set; }
        public string board_type { get; set; }
        public DateTime confirm_date { get; set; }

        // Yeni eklenen alanlar
        public int user_id { get; set; }  // Kullanıcı ID'si

        [ForeignKey("user_id")]
        public User User { get; set; }  // User ile ilişki

        [ForeignKey("reservation_request_id")]
        public ReservationRequest ReservationRequest { get; set; }

        [ForeignKey("hotel_id")]
        public Hotel Hotel { get; set; }
    }
}
