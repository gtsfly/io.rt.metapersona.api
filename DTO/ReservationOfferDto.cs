namespace otel_advisor_webApp.DTOs
{
    public class ReservationOfferDto
    {
        public string offer_key { get; set; }
        public DateTime check_in_date { get; set; }
        public DateTime check_out_date { get; set; }
        public decimal price { get; set; }
        public string room_type { get; set; }
        public string board_type{ get; set; }
    }
}
