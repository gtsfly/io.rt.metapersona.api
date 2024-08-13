namespace otel_advisor_webApp.Models
{
    public class UserPreference
    {
        public int user_preference_id { get; set; }
        public int user_id { get; set; }
        public int experience_id { get; set; }
        public int priority { get; set; } 
        public Experience experience { get; set; }
        public User user { get; set; }
    }
}
