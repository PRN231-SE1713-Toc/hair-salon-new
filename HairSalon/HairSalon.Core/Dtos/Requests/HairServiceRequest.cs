using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Dtos.Requests
{
    /// <summary>
    /// Create model for hair service
    /// </summary>
    public class HairServiceRequest
    {
        [Required(ErrorMessage = "Service's name is required!", AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required!", AllowEmptyStrings = false)]
        [StringLength(300, ErrorMessage = "The max length of description is 300 characters!")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required!", AllowEmptyStrings = false)]
        public string? EstimatedDuration { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }
    }
}
