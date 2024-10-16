using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Entities
{
    public class StylistFeedback : IEntity
    {
        public int Id { get; set; }

        public int StylistId { get; set; }

        public int CustomerId { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        /// <summary>
        /// Stylist rating from 1 to 5
        /// </summary>
        public short Rating { get; set; }

        public DateTime FeedbackDate { get; set; }

        public Employee Stylist { get; set; } = null!;

        public Customer Customer { get; set; } = null!;
    }
}
