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
        Task CreateAsync(Employee employee);
        Task RemoveAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        bool EmployeeExists(int id);
    }
}
