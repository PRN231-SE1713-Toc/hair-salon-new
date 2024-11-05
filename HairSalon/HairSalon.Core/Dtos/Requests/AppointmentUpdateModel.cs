using HairSalon.Core.Entities;
using HairSalon.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class AppointmentUpdateModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must greater than 0")]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Customer Id must greater than 0")]
        public int CustomerId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stylist Id must greater than 0")]
        public int StylistId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [StringLength(255)]
        public string? Note { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Required]
        public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

        //public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
