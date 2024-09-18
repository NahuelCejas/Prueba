using Xunit;
using Moq;
using FluentAssertions;
using Application.UseCase.InteractionTypeServices;
using Application.Interfaces.IQuery;
using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace UnitTest.UseCase.InteractionTypeServices
{
    public class InteractionTypeGetServicesTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnListOfGenericResponse_WhenQueryReturnsData()
        {
            // Arrange
            var mockQuery = new Mock<IInteractionTypeQuery>();
            var mockData = new List<InteractionType> 
        {
            new InteractionType { Id = 1, Name = "Initial Meeting" }, 
            new InteractionType { Id = 2, Name = "Phone Call" }
        };

            mockQuery.Setup(q => q.GetListInteractionTypes()).ReturnsAsync(mockData);

            var service = new InteractionTypeGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll();    

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1); 
            result[0].Name.Should().Be("Initial Meeting");
            result[1].Id.Should().Be(2);
            result[1].Name.Should().Be("Phone Call");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenQueryReturnsNoData()
        {
            // Arrange
            var mockQuery = new Mock<IInteractionTypeQuery>();
            mockQuery.Setup(q => q.GetListInteractionTypes()).ReturnsAsync(new List<InteractionType>());   // Empty list of entities

            var service = new InteractionTypeGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll();    // Should return an empty list 

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
