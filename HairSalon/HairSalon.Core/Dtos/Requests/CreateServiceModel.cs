using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class CreateServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string? Duration { get; set; }
        public decimal Price { get; set; }
    }
}
