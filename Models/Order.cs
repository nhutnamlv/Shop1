namespace ShopApi.Models
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public int Status { get; set; } = 0;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Payment? Payment { get; set; }
    }
}