namespace ShopApi.Models
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}