using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;
using StaffApi.Models;
using StaffApi.DTO;

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
        /// Gets list of all employees with their positions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            return employees.Select(e => new EmployeeDTO(e)).ToList();
        }

        /// <summary>
        /// Gets specific employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return new EmployeeDTO(employee);
        }

        /// <summary>
        /// Updates specified employee info
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Employee
        ///     {
        ///        "id": "0",
        ///        "name": "Another Employee Name",
        ///        "dateOfBirth": "1995-10-15"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates new employee
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Employee
        ///     {        
        ///        "name": "New Employee Name",
        ///        "dateOfBirth": "1995-10-20"
        ///     }
        ///
        /// </remarks>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Create(Employee employee)
        {
            await _employeeRepository.CreateAsync(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeDTO(employee));
        }

        /// <summary>
        /// Deletes specified employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds position to specified employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpPost("{employeeId}/Position/{positionId}")]
        public async Task<ActionResult<EmployeeDTO>> AddPosition(int employeeId, int positionId)
        {
            var employee = await _employeeRepository.FindAsync(employeeId);
            if (employee == null)
                return NotFound();
            var position = await _positionRepository.FindAsync(positionId);
            if (position == null)
                return NotFound();
            try
            {
                await _employeeRepository.AddPositionAsync(employee, position);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Specified employee does not have such position");
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeDTO(employee));
        }

        /// <summary>
        /// Removes position from specified employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpDelete("{employeeId}/Position/{positionId}")]
        public async Task<ActionResult<EmployeeDTO>> RemovePosition(int employeeId, int positionId)
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
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeDTO(employee));
        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepository.EmployeeExists(id);
        }
    }
}
