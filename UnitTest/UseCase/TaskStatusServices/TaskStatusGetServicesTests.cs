using Xunit;
using Moq;
using FluentAssertions;
using Application.UseCase.TaskStatusServices;
using Application.Interfaces.IQuery;
using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Domain.Entities.TaskStatus;

namespace UnitTest.UseCase.TaskStatusServices
{
    public class TaskStatusGetServicesTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnListOfGenericResponse_WhenQueryReturnsData()
        {
            // Arrange
            var mockQuery = new Mock<ITaskStatusQuery>();
            var mockData = new List<TaskStatus> // List of entities
        {
            new TaskStatus { Id = 1, Name = "Pending" }, 
            new TaskStatus { Id = 2, Name = "In Progress" }
        };

            mockQuery.Setup(q => q.GetListTaskStatus()).ReturnsAsync(mockData);

            var service = new TaskStatusGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll(); 

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1); 
            result[0].Name.Should().Be("Pending");
            result[1].Id.Should().Be(2);
            result[1].Name.Should().Be("In Progress");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenQueryReturnsNoData()
        {
            // Arrange
            var mockQuery = new Mock<ITaskStatusQuery>();
            mockQuery.Setup(q => q.GetListTaskStatus()).ReturnsAsync(new List<TaskStatus>()); // Empty list of entities

            var service = new TaskStatusGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll(); // Should return an empty list 

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
