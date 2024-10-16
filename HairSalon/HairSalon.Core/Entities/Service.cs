using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Entities
{
    public class Service : IEntity
    {
        public int Id { get; set; }

        [Column("ServiceName", TypeName = "nvarchar")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("ServiceDescription", TypeName = "nvarchar")]
        [StringLength(300)]
        public string Description { get; set; } = null!;

        [Column("EstimatedDuration", TypeName = "nvarchar(100)")]
        public string? Duration { get; set; }

        [Column(TypeName = "decimal(10)")]
        public decimal Price { get; set; }

        public ICollection<AppointmentService> AppointmentServices { get; set; } = null!;
    }
}
