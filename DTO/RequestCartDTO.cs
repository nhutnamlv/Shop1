
namespace Shop1.DTO
{
    public class RequestPaymenDTO
    {
        public int UserId { get; set; }
        public int OrderId { get; internal set; }
        public string PaymentMethod { get; internal set; }
        public int PaymentStatus { get; internal set; }
        public DateTime? PaidAt { get; internal set; }
    }
}
