namespace HairSalon.Core.Dtos.Responses
{
    public class CustomerResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }
    }
}
