namespace Shop1.DTO
{
    public class RequestCheckoutDTO
    {
        public int UserId { get; set; }

        public string Address { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string PaymentMethod { get; set; } = "COD";
    }
}
