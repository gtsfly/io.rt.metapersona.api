using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace otel_advisor_webApp.Models
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int hotel_id { get; set; }
        public string name { get; set; }
        public int location_id { get; set; }  // Foreign key
        public double rating { get; set; }

        [ForeignKey("location_id")]
        public Location Location { get; set; }

        public ICollection<HotelExperience> HotelExperiences { get; set; } = new List<HotelExperience>();
        public ICollection<ReservationRequest> ReservationRequests { get; set; } = new List<ReservationRequest>();
        public ICollection<ReservationOffer> ReservationOffers { get; set; } = new List<ReservationOffer>();

    }
}
