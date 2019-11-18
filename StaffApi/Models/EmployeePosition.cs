using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.Models
{
    public class EmployeePosition
    {
        public uint EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public uint PositionId { get; set; }
        public Position Position { get; set; }
    }
}
