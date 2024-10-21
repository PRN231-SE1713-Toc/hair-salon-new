using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class UpdateAppointmentServiceModel
    {
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
