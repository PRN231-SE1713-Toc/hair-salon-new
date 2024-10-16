namespace HairSalon.Core.Dtos.Requests
{
    public class UpdatedCustomer
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        //public string Password { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }
    }
}
