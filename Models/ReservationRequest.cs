using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace otel_advisor_webApp.Models
{
    public class ReservationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reservation_id { get; set; }
        public int user_id { get; set; }
        public DateTime trip_start { get; set; }
        public DateTime trip_end { get; set; }
        public decimal budget { get; set; }
        public string location { get; set; }
        public int stay_duration { get; set; }
        public string exp_1 { get; set; }
        public int exp_1_rating { get; set; }
        public string exp_2 { get; set; }
        public int exp_2_rating { get; set; }
        public string exp_3 { get; set; }
        public int exp_3_rating { get; set; }
        public User user { get; set; }
    }
}
