using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class CreateEmployeeScheduleModel
    {
        public int EmployeeId { get; set; }

        public DateOnly WorkingDate { get; set; }

        public TimeOnly WorkingStartTime { get; set; }

        public TimeOnly WorkingEndTime { get; set; }
    }
}
