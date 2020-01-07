using System.Collections.Generic;
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
        Task AddPositionAsync(Employee employee, Position position);
        Task RemovePositionAsync(Employee employee, Position position);
    }
}