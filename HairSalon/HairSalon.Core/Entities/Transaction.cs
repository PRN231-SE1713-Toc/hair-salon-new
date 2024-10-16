using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Entities
{
    public class Transaction : IEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int AppointmentId { get; set; }

        public DateTime TransactionDate { get; set; }

        [StringLength(50)]
        public string? Method { get; set; }

        //TODO: Need to review the return status of payment gateway
        public int Status { get; set; }

        [Column(TypeName = "decimal(10)")]
        public decimal Amount { get; set; }

        public Appointment Appointment { get; set; } = null!;

        public Customer Customer { get; set; } = null!;
    }
}
