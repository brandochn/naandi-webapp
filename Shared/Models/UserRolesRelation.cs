namespace Naandi.Shared.Models
{
    public class UserRolesRelation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RolesId { get; set; }
        public Roles Roles { get; set; }
        public bool Active { get; set; }
    }
}