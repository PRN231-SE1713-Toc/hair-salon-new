namespace HairSalon.Core.Dtos.Responses
{
    public class LoginEmployeeResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = null!;
    }
}
