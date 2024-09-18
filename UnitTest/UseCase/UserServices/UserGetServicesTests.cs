using Xunit;
using Moq;
using FluentAssertions;
using Application.UseCase.UserServices;
using Application.Interfaces.IQuery;
using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace UnitTest.UseCase.UserServices
{
    public class UserGetServicesTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnListOfUsers_WhenQueryReturnsData()
        {
            // Arrange
            var mockQuery = new Mock<IUserQuery>();
            var mockData = new List<User> // List of entities
        {
            new User { UserID = 1, Name = "Rocio Kreick", Email = "rociokreick@gmail.com" },
            new User { UserID = 2, Name = "Giuliana Nicolosi", Email = "giuliananicolosi@gmail.com" }
        };

            mockQuery.Setup(q => q.GetListUsers()).ReturnsAsync(mockData);

            var service = new UserGetServices(mockQuery.Object);

            // Act
            List<Users> result = await service.GetAll(); // Should return a list of 'Users' response 

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1); 
            result[0].Name.Should().Be("Rocio Kreick");
            result[0].Email.Should().Be("rociokreick@gmail.com");
            result[1].Id.Should().Be(2);
            result[1].Name.Should().Be("Giuliana Nicolosi");
            result[1].Email.Should().Be("giuliananicolosi@gmail.com");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenQueryReturnsNoData()
        {
            // Arrange
            var mockQuery = new Mock<IUserQuery>();
            mockQuery.Setup(q => q.GetListUsers()).ReturnsAsync(new List<User>()); // Empty list of entities

            var service = new UserGetServices(mockQuery.Object);

            // Act
            List<Users> result = await service.GetAll(); // Should return an empty list 

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
