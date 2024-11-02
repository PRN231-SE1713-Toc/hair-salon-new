using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class CreateAppointmentServiceModel
    {
        public int ServiceId { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
