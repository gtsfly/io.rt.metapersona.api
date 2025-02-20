using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace otel_advisor_webApp.Models
{
    public class UserHotelExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_hotel_experience_id { get; set; }

        public int user_id { get; set; }

        public int hotel_id { get; set; }

        public int reservation_request_id { get; set; }

        [Range(1, 10)]
        public int overall_rating { get; set; }

        public string experience_1 { get; set; }
        [Range(1, 10)]
        public int experience_1_rating { get; set; }

        public string experience_2 { get; set; }
        [Range(1, 10)]
        public int experience_2_rating { get; set; }

        public string experience_3 { get; set; }
        [Range(1, 10)]
        public int experience_3_rating { get; set; }

        public string most_liked_experience { get; set; }

        public string least_liked_experience { get; set; }

        public bool would_visit_again { get; set; }

        [StringLength(500)]
        public string additional_comments { get; set; }

        public DateTime created_at { get; set; }

        [ForeignKey("user_id")]
        public User User { get; set; }

        [ForeignKey("hotel_id")]
        public Hotel Hotel { get; set; }

        [ForeignKey("reservation_request_id")]
        public ReservationConfirmed ReservationConfirmed { get; set; }
    }
}