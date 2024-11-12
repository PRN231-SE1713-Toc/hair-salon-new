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

        public string WorkingStartTime { get; set; }

        public string WorkingEndTime { get; set; }
    }
}
