using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Core.Entities
{
    public class EmployeeSchedule : IEntity
    {
        [Column("EmpScheduleId")]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateOnly WorkingDate { get; set; }

        public TimeOnly WorkingStartTime { get; set; }

        public TimeOnly WorkingEndTime { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
