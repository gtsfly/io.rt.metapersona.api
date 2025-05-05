using System.Collections.Generic;
using otel_advisor_webApp.Models;

namespace otel_advisor_webApp.DTO
{
    public class RecommendationRequestDto
    {
        public List<ExperiencePreference> experience_preferences { get; set; }
        public string user_email { get; set; }
        public int? location_id { get; set; }
    }
} 