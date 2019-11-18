using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.Models
{
    public class EmployeePosition
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
