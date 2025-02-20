using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace otel_advisor_webApp.Models
{
    public class User
    {
        
        [DatabaseGenerat‌ed(DatabaseGeneratedOp‌​tion.Identity)]
        public int user_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        public ICollection<ReservationRequest> ReservationRequests { get; set; } = new List<ReservationRequest>();
        public ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
    }
}
