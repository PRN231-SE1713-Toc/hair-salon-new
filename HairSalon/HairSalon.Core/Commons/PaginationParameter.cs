using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HairSalon.Core.Commons
{
    public class PaginationParameter
    {
        const int maxPageSize = 50;
        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        [JsonIgnore]

        public int Limit
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
