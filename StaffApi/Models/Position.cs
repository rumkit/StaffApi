using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.Models
{
    public class Position
    {
        [Key]
        public uint Id { get; set; }
        public string Name { get; set; }
        [Range(1,15)]
        public int Grade { get; set; }
        public ICollection<EmployeePosition> EmployeePositions { get; set; }        
    }
}
