using System;
using System.Collections.Generic;
using System.Linq;
using StaffApi.Models;

namespace StaffApi.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<PositionDTO> Positions { get; set; }

        public EmployeeDTO()
        {
        }

        public EmployeeDTO(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            DateOfBirth = employee.DateOfBirth;
            if (employee.EmployeePositions?.Count > 0)
            {
                Positions = employee.EmployeePositions.Select(ep => new PositionDTO(ep.Position)).ToList();
            }
        }
    }
}