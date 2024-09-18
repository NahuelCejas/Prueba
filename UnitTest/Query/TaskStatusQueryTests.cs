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
    public class TaskStatusQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetListTaskStatus_ShouldReturnAllTaskStatuses_WhenTheyExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new TaskStatusQuery(context);

                // Insertar datos de prueba
                context.Taskstatus.AddRange(
                    new Domain.Entities.TaskStatus { Id = 1, Name = "Pending" },
                    new Domain.Entities.TaskStatus { Id = 2, Name = "In Progress" }
                );
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetListTaskStatus();

                // Assert
                result.Should().NotBeNull();
                result.Should().HaveCount(2);
                result.Should().Contain(ts => ts.Name == "Pending");
                result.Should().Contain(ts => ts.Name == "In Progress");
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListTaskStatus_ShouldReturnEmptyList_WhenNoTaskStatusesExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new TaskStatusQuery(context);

                // Act
                var result = await service.GetListTaskStatus();

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEmpty();
            }
        }
    }
}
