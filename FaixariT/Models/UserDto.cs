namespace FaixariT.Models
{
    public class User
    {
        public int UserId { get; set; }  // Primary key for EF Core
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
