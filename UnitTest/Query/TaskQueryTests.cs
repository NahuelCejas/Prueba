using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Query;
using Infrastructure.Persistence;
using Domain.Entities;
using Application.Exceptions;
using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace UnitTest.Query
{
    public class TaskQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new TaskQuery(context);

                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Project A"
                };

                var user = new User
                {
                    UserID = 1,
                    Name = "User1",
                    Email = "user1@gmail.com"
                };

                var taskStatus = new Domain.Entities.TaskStatus
                {
                    Id = 1,
                    Name = "Pending"
                };

                var task = new Domain.Entities.Task
                {
                    TaskID = Guid.NewGuid(),
                    Name = "Sample Task",
                    DueDate = DateTime.Now.AddDays(5),
                    CreateDate = DateTime.Now,
                    ProjectID = project.ProjectID,
                    AssignedTo = user.UserID,
                    Status = taskStatus.Id,
                    Projects = project,
                    Users = user,
                    TaskStatus = taskStatus
                };

                context.Tasks.Add(task);
                await context.SaveChangesAsync();

                // Act
                Domain.Entities.Task result = await service.GetTaskById(task.TaskID);

                // Assert
                result.Should().NotBeNull();
                result.TaskID.Should().Be(task.TaskID);
                result.Name.Should().Be(task.Name);
                result.ProjectID.Should().Be(task.ProjectID);
                result.AssignedTo.Should().Be(task.AssignedTo);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new TaskQuery(context);
                var nonExistentTaskId = Guid.NewGuid();

                // Act
                Func<System.Threading.Tasks.Task> act = async () => { await service.GetTaskById(nonExistentTaskId); };

                // Assert
                await act.Should().ThrowAsync<NotFoundException>().WithMessage("Task not found");
            }
        }
    }
}
