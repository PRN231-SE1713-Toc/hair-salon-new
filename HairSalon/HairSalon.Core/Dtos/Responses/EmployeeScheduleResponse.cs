using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Responses
{
    public class EmployeeScheduleResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly WorkingDate { get; set; }

        public TimeOnly WorkingStartTime { get; set; }

        public TimeOnly WorkingEndTime { get; set; }
    }
}
