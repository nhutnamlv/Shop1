namespace Shop1.DTO
{
    public class RequestPaymentDTO
    {
        public int OrderId { get; set; }

        public string PaymentMethod { get; set; } = "";

        public int PaymentStatus { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
