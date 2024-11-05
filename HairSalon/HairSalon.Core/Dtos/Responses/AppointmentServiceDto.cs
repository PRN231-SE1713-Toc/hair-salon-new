using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Responses
{
    public class AppointmentServiceDto
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }

        public string? ServiceName { get; set; }

        [Column(TypeName = "decimal(10)")]
        public decimal CurrentPrice { get; set; }
    }
}
