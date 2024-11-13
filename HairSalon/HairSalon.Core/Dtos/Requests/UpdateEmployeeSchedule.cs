using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class UpdateEmployeeSchedule
    {
        public int EmployeeId { get; set; }

        public DateOnly WorkingDate { get; set; }

        public string WorkingStartTime { get; set; }

        public string WorkingEndTime { get; set; }
    }
}
