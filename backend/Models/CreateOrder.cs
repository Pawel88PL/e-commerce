namespace backend.Models
{
    public class CreateOrder
    {
        public Guid CartId { get; set; }
        public string? UserId { get; set; }
        public bool IsPickupInStore { get; set; }
        public string? Client_ip { get; set; }
    }
}