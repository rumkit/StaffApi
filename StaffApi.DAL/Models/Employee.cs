using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StaffApi.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<EmployeePosition> EmployeePositions {get;set;}        
    }
}
