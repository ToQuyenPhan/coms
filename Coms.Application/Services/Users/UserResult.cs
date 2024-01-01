namespace Coms.Application.Services.Users
{
    public class UserResult
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public DateTime Dob { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
    }
}
