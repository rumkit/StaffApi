using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StaffApi.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(1, 15)]
        public int Grade { get; set; }
        public virtual ICollection<EmployeePosition> EmployeePositions { get; set; }
    }
}