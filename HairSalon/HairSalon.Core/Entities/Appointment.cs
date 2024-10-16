using HairSalon.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int StylistId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        [StringLength(255)]
        public string? Note { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Customer Customer { get; set; } = null!;

        public Employee Stylist { get; set; } = null!;
    }
}
