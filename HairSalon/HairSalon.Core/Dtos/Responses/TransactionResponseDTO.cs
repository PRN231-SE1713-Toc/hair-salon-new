using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Responses
{
    public class TransactionResponseDTO
    {
        public int CustomerId { get; set; }
        public int AppointmentId { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
