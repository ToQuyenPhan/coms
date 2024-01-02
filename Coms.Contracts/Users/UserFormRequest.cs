namespace Coms.Contracts.Users
{
    public class UserFormRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }

        public string? Image { get; set; }
        public DateTime Dob { get; set; }
        public int RoleId { get; set; }
    }
}
