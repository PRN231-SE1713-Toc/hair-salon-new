using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.PaginationDtos
{
    public class CustomerFilterDTO
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Search { get; set; }
    }
}
