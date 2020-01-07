using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;

namespace StaffApi.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly StaffApiContext _context;

        public async Task<Employee> FindAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task AddPositionAsync(Employee employee, Position position)
        {
            employee.EmployeePositions.Add(new EmployeePosition
            {
                EmployeeId = employee.Id,
                PositionId = position.Id
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemovePositionAsync(Employee employee, Position position)
        {
            var ep = employee.EmployeePositions.FirstOrDefault(ep => ep.PositionId == position.Id);
            if (ep == null)
                throw new DbUpdateConcurrencyException();
            employee.EmployeePositions.Remove(ep);
            await _context.SaveChangesAsync();
        }

        public EmployeeRepository(StaffApiContext context)
        {
            _context = context;
        }
    }
}