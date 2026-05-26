namespace Shop1.DTO
{
    public class RequestOrderDTO
    {
        public int UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Address { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public int Status { get; set; }
    }
}
