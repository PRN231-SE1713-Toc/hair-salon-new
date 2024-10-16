using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Dtos.Requests
{
    public class CreatedCustomerModel
    {
        // TODO: Add more validation meeting the requirements
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer's name is required!")]
        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        
        public DateTime? DateOfBirth { get; set; }
    }
}
