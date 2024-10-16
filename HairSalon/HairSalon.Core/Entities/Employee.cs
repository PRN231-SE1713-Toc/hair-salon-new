using HairSalon.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Core.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Column("EmployeeName")]
        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [StringLength(10)]
        public string? PhoneNumber { get; set; }

        [StringLength(12)]
        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public EmployeeRole Role { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<StylistFeedback> StylistFeedback { get; set; } = new List<StylistFeedback>();

        public ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();
    }
}
