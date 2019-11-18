using Microsoft.EntityFrameworkCore;
using StaffApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public EmployeeRepository(StaffApiContext context)
        {
            _context = context;
        }
    }
}
