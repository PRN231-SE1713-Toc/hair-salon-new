using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Dtos.Requests
{
    public class UpdateServiceRequest
    {

        [Column("ServiceName", TypeName = "nvarchar")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("ServiceDescription", TypeName = "nvarchar")]
        [StringLength(300)]
        public string Description { get; set; } = null!;

        [Column("EstimatedDuration", TypeName = "nvarchar(100)")]
        public string? Duration { get; set; }

        [Column(TypeName = "decimal(10)")]
        public decimal Price { get; set; }
    }
}
