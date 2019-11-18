using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.Models
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> FindAsync(int id);
        Task CreateEmployeeAsync(Employee employee);
        Task RemoveEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        bool EmployeeExists(int id);
    }
}
