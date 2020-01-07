using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StaffApi.Controllers;
using StaffApi.DTO;
using StaffApi.Models;
using Xunit;

namespace StaffApi.Tests
{
    public class EmployeesControllerTest
    {
        readonly EmployeesController _controller;

        public EmployeesControllerTest()
        {
            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.GetEmployeesAsync()).ReturnsAsync(GetEmployeeCollection);
            employeeRepository.Setup(x => x.FindAsync(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(GetEmployeeCollection().FirstOrDefault(e => e.Id == id)));
            var positionRepository = new Mock<IPositionRepository>();
            _controller = new EmployeesController(employeeRepository.Object, positionRepository.Object);
        }

        [Fact]
        public async void GetEmployees()
        {
            var response = await _controller.GetEmployees();
            var responseValue = Assert.IsAssignableFrom<IEnumerable<EmployeeDTO>>(response.Value);
            Assert.Equal(3, responseValue.Count());
        }

        [Fact]
        public async void GetEmployee()
        {
            var response = await _controller.GetEmployee(3);
            var responseValue = Assert.IsAssignableFrom<EmployeeDTO>(response.Value);
            Assert.Equal("Test2", responseValue.Name);
        }

        [Fact]
        public async void GetNonExisting()
        {
            var response = await _controller.GetEmployee(4);
            var responseResult = Assert.IsAssignableFrom<StatusCodeResult>(response.Result);
            Assert.Equal(responseResult.StatusCode, (int) HttpStatusCode.NotFound);
        }

        [Fact]
        public async void PutWrongId()
        {
            var response = await _controller.Update(99, new Employee {Id = 2});
            var responseResult = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(responseResult.StatusCode, (int) HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void DeleteNonExisting()
        {
            var response = await _controller.Delete(99);
            var responseResult = Assert.IsAssignableFrom<StatusCodeResult>(response.Result);
            Assert.Equal(responseResult.StatusCode, (int) HttpStatusCode.NotFound);
        }


        private IEnumerable<Employee> GetEmployeeCollection()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "Test1",
                    DateOfBirth = DateTime.Parse("01/01/1989", CultureInfo.InvariantCulture)
                },
                new Employee
                {
                    Id = 3,
                    Name = "Test2",
                    DateOfBirth = DateTime.Parse("10/08/1995", CultureInfo.InvariantCulture)
                },
                new Employee
                {
                    Id = 7,
                    Name = "Test3",
                    DateOfBirth = DateTime.Parse("03/16/1975", CultureInfo.InvariantCulture)
                }
            };
        }
    }
}