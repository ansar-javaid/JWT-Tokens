namespace JWT_Practice.Models
{
    public class User
    {
        public string UserName { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
