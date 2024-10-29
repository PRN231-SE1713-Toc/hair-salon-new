    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Responses
{
    public class ServiceDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string? Duration { get; set; }
        public decimal Price { get; set; }
        public ICollection<AppointmentServiceDto> AppointmentServices { get; set; }
    }
}
