namespace ShopApi.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}