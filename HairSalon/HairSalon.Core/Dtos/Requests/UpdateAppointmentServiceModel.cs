using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class UpdateAppointmentServiceModel
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must greater than 0")]
        public decimal CurrentPrice { get; set; }
    }
}
