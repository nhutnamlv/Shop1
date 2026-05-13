namespace ShopApi.Models
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}