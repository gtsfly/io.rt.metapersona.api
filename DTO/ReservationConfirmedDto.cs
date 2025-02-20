namespace otel_advisor_webApp.DTOs

{
    public class ReservationConfirmedDto
    {
        public int confirmed_reservation_id { get; set; }
        public int reservation_request_id { get; set; }
        public int hotel_id { get; set; }
        public DateTime check_in_date { get; set; }
        public DateTime check_out_date { get; set; }
        public int price { get; set; }
        public string room_type { get; set; }
        public string board_type {  get; set; }
        public DateTime confirm_date { get; set; }

        public int user_id { get; set; }   
        public string user_name { get; set; } 
    }
}
