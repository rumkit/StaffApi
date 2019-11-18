using StaffApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Position> Positions { get; set; }

        public EmployeeViewModel()
        {

        }

        public EmployeeViewModel(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            DateOfBirth = employee.DateOfBirth;
            if (employee.EmployeePositions?.Count > 0)
            {
                Positions = employee.EmployeePositions.Select(ep => ep.Position).ToList();
            }
        }
    }
}
