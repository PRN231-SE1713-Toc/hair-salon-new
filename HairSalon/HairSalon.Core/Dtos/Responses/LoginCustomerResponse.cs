namespace HairSalon.Core.Dtos.Responses
{
    /// <summary>
    /// Login response model for customer
    /// </summary>
    public class LoginCustomerResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
