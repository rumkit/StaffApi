using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;
using StaffApi.Models;
using StaffApi.ViewModels;

namespace StaffApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

        }

        /// <summary>
        /// Get list of all employees with their positions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployee()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            return employees.Select(e => new EmployeeViewModel(e)).ToList();
        }

        /// <summary>
        /// Get employee with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return new EmployeeViewModel(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            try
            {
                await _employeeRepository.UpdateAsync(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            await _employeeRepository.CreateAsync(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.RemoveAsync(employee);
            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepository.EmployeeExists(id);
        }
    }
}
