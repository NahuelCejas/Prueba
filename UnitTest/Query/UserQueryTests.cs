using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Query;
using Infrastructure.Persistence;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace UnitTest.Query
{
    public class UserQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetListUsers_ShouldReturnAllUsers_WhenTheyExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new UserQuery(context);

                // Insertar datos de prueba
                context.Users.AddRange(
                    new User { UserID = 1, Name = "User 1", Email = "user1@gmail.com" },
                    new User { UserID = 2, Name = "User 2", Email = "user2@gmail.com" }
                );
                await context.SaveChangesAsync();

                // Act
                List<User> result = await service.GetListUsers();

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(2);
                result.Should().Contain(u => u.Name == "User 1" && u.Email == "user1@gmail.com");
                result.Should().Contain(u => u.Name == "User 2" && u.Email == "user2@gmail.com");
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListUsers_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new UserQuery(context);

                // Act
                List<User> result = await service.GetListUsers();

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEmpty();
            }
        }
    }
}
