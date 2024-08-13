using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace otel_advisor_webApp.Models
{
    public class Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int experience_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public ICollection<HotelExperience> HotelExperiences { get; set; } = new List<HotelExperience>();
        public ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();

    }
}
