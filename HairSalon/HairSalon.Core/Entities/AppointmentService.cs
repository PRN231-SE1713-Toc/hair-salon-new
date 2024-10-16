using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Core.Entities
{
    public class AppointmentService : IEntity
    {
        [Column("AppServiceId")]
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }

        [Column(TypeName = "decimal(10)")]
        public decimal CurrentPrice { get; set; }

        public Appointment Appointment { get; set; } = null!;

        public Service Service { get; set; } = null!;
    }
}
