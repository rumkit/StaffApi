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
        private readonly IPositionRepository _positionRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IPositionRepository positionRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;

        }

        /// <summary>
        /// Get list of all employees with their positions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployees()
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
        public async Task<IActionResult> Update(int id, Employee employee)
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
        public async Task<ActionResult<EmployeeViewModel>> Create(Employee employee)
        {
            await _employeeRepository.CreateAsync(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeViewModel(employee));
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var employee = await _employeeRepository.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.RemoveAsync(employee);
            return employee;
        }

        [HttpPost("{employeeId}/Position/{positionId}")]
        public async Task<ActionResult<EmployeeViewModel>> AddPosition(int employeeId, int positionId)
        {
            var employee = await _employeeRepository.FindAsync(employeeId);
            if(employee == null)
                return NotFound();
            var position = await _positionRepository.FindAsync(positionId);
            if(position == null)
                return NotFound();
            try
            {
                await _employeeRepository.AddPositionAsync(employee, position);
            }
            catch(InvalidOperationException)
            {                
                return BadRequest("Specified employee does not have such position");
            }
            
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeViewModel(employee));
        }


        [HttpDelete("{employeeId}/Position/{positionId}")]
        public async Task<ActionResult<EmployeeViewModel>> RemovePosition(int employeeId, int positionId)
        {
            var employee = await _employeeRepository.FindAsync(employeeId);
            if (employee == null)
                return NotFound();
            var position = await _positionRepository.FindAsync(positionId);
            if (position == null)
                return NotFound();
            try
            {
                await _employeeRepository.RemovePositionAsync(employee, position);
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeViewModel(employee));
        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepository.EmployeeExists(id);
        }
    }
}
