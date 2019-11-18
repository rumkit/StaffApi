using Microsoft.AspNetCore.Mvc;
using Moq;
using StaffApi.Controllers;
using StaffApi.Models;
using StaffApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StaffApi.Tests
{
    public class EmployeesControllerTest
    {
        readonly EmployeesController _controller;
        public EmployeesControllerTest()
        {
            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.GetEmployeesAsync()).ReturnsAsync(GetEmployees);
            employeeRepository.Setup(x => x.FindAsync(It.IsAny<int>())).Returns<int>((id) => Task.FromResult(GetEmployees().FirstOrDefault(e => e.Id == id)));
            var positionRepository = new Mock<IPositionRepository>();
            _controller = new EmployeesController(employeeRepository.Object, positionRepository.Object);
        }

        [Fact]
        public async void GetEmployee()
        {
            var response = await _controller.GetEmployees();
            var responseValue = Assert.IsAssignableFrom<IEnumerable<EmployeeViewModel>>(response.Value);
            Assert.Equal(3, responseValue.Count());
        }

        [Fact]
        public async void GetEmployeeId()
        {
            var response = await _controller.GetEmployee(3);
            var responseValue = Assert.IsAssignableFrom<EmployeeViewModel>(response.Value);
            Assert.Equal("Test2", responseValue.Name);
        }

        [Fact]
        public async void GetNonExistingEmployee()
        {
            var response = await _controller.GetEmployee(4);
            var responseResult = Assert.IsAssignableFrom<StatusCodeResult>(response.Result);
            Assert.Equal(responseResult.StatusCode, (int)HttpStatusCode.NotFound);
        }


        private IEnumerable<Employee> GetEmployees()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    Name = "Test1",
                    DateOfBirth = DateTime.Parse("1.01.1989")
                },
                new Employee()
                {
                    Id = 3,
                    Name = "Test2",
                    DateOfBirth = DateTime.Parse("8.10.1995")
                },
                new Employee()
                {
                    Id = 7,
                    Name = "Test3",
                    DateOfBirth = DateTime.Parse("16.03.1975")
                }
            };
        }

    }
}
