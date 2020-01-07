using System.Collections.Generic;
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
    public class PositionsControllerTests
    {
        readonly PositionsController _controller;

        public PositionsControllerTests()
        {
            var repository = new Mock<IPositionRepository>();
            repository.Setup(x => x.GetPositionsAsync()).ReturnsAsync(GetPositionsCollection);
            repository.Setup(x => x.FindAsync(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(GetPositionsCollection().FirstOrDefault(e => e.Id == id)));

            _controller = new PositionsController(repository.Object);
        }

        [Fact]
        public async void GetPositions()
        {
            var response = await _controller.GetPositions();
            var responseValue = Assert.IsAssignableFrom<IEnumerable<PositionDTO>>(response.Value);
            Assert.Equal(3, responseValue.Count());
        }

        [Fact]
        public async void GetPosition()
        {
            var response = await _controller.GetPosition(3);
            var responseValue = Assert.IsAssignableFrom<PositionDTO>(response.Value);
            Assert.Equal("Test Position 2", responseValue.Name);
        }

        [Fact]
        public async void GetNonExisting()
        {
            var response = await _controller.GetPosition(4);
            var responseResult = Assert.IsAssignableFrom<StatusCodeResult>(response.Result);
            Assert.Equal(responseResult.StatusCode, (int) HttpStatusCode.NotFound);
        }

        [Fact]
        public async void PutWrongId()
        {
            var response = await _controller.Update(99, new Position {Id = 2});
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

        private IEnumerable<Position> GetPositionsCollection()
        {
            return new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Name = "Test Position 1",
                    Grade = 10
                },
                new Position
                {
                    Id = 3,
                    Name = "Test Position 2",
                    Grade = 10
                },
                new Position
                {
                    Id = 5,
                    Name = "Test Position 3",
                    Grade = 10
                }
            };
        }
    }
}