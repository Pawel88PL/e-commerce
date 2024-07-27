namespace backend.Models
{
    public class ServiceResponse
    {
        public string? Pos_id { get; set; }
        public string? Order_id { get; set; }
        public string? Session_id { get; set; }
        public string? Amount { get; set; }
        public int? Response_code { get; set; }
        public string? Transaction_id { get; set; }
        public string? Cc_number_hash { get; set; }
        public string? Bin { get; set; }
        public string? Card_type { get; set; }
        public string? Auth_code { get; set; }
        public string? ControlData { get; set; }
    }
}