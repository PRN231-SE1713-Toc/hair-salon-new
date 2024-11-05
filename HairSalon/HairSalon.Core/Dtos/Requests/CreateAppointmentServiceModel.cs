using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class CreateAppointmentServiceModel
    {
        [Required]
        public int ServiceId { get; set; }

    }
}
