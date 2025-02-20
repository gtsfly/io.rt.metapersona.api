using System;
using System.ComponentModel.DataAnnotations;

namespace otel_advisor_webApp.DTO
{
    public class UserHotelExperienceDto
    {
        public int user_hotel_experience_id { get; set; }

        [Required]
        public int user_id { get; set; }

        public string user_name { get; set; } 

        [Required]
        public int hotel_id { get; set; }

        [Required]
        public int reservation_request_id { get; set; }

        [Required]
        [Range(1, 10)]
        public int overall_rating { get; set; }

        [Required]
        public string experience_1 { get; set; }

        [Required]
        [Range(1, 10)]
        public int experience_1_rating { get; set; }

        [Required]
        public string experience_2 { get; set; }

        [Required]
        [Range(1, 10)]
        public int experience_2_rating { get; set; }

        [Required]
        public string experience_3 { get; set; }

        [Required]
        [Range(1, 10)]
        public int experience_3_rating { get; set; }

        [Required]
        public string most_liked_experience { get; set; }

        [Required]
        public string least_liked_experience { get; set; }

        [Required]
        public bool would_visit_again { get; set; }

        [StringLength(500)]
        public string additional_comments { get; set; }

        public DateTime created_at { get; set; }
    }
}
