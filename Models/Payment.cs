namespace ShopApi.Models
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string? PaymentMethod { get; set; }
        public int PaymentStatus { get; set; } = 0;

        public DateTime? PaidAt { get; set; }
    }
}