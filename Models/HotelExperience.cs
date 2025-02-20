using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace otel_advisor_webApp.Models
{
    public class HotelExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int hotel_id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int experience_id { get; set; }

        public int rating { get; set; }

        [ForeignKey("hotel_id")]
        public Hotel hotel { get; set; }

        [ForeignKey("experience_id")]
        public Experience experience { get; set; }
    }
}
