namespace otel_advisor_webApp.DTOs
{
    public class ReservationRequestDto
    {
        public int reservation_request_id { get; set; }
        public int user_id { get; set; }
        public string? user_name { get; set; }
        public DateTime check_in_range_start { get; set; }
        public DateTime check_in_range_end { get; set; }
        public decimal budget { get; set; }
        public string location { get; set; }
        public int stay_duration { get; set; }
        public string exp_1 { get; set; }
        public int exp_1_rating { get; set; }
        public string exp_2 { get; set; }
        public int exp_2_rating { get; set; }
        public string exp_3 { get; set; }
        public int exp_3_rating { get; set; }
        public int adult_num { get; set; }
        public int child_num { get; set; }
        public List<int> children_ages { get; set; }
        public DateTime created_at { get; set; }
    }
}
