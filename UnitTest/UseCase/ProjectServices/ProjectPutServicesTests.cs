using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IValidator;
using Application.Models;
using Application.UseCase.ProjectServices;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UseCase.ProjectServices
{
    public class ProjectPutServicesTests
    {
        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var mockProjectCommand = new Mock<IProjectCommand>();
            var mockTaskQuery = new Mock<ITaskQuery>();
            var mockValidator = new Mock<IValidatorHandler<TasksRequest>>();

            var service = new ProjectPutServices(mockProjectCommand.Object, mockTaskQuery.Object, mockValidator.Object);

            var taskId = Guid.NewGuid();
            var request = new TasksRequest();

            mockTaskQuery.Setup(q => q.GetTaskById(taskId)).ReturnsAsync((Domain.Entities.Task)null);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => { await service.UpdateTask(taskId, request); };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Task not found");
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldUpdateTask_WhenTaskExistsAndRequestIsValid()
        {
            // Arrange
            var mockProjectCommand = new Mock<IProjectCommand>();
            var mockTaskQuery = new Mock<ITaskQuery>();
            var mockValidator = new Mock<IValidatorHandler<TasksRequest>>();

            var service = new ProjectPutServices(mockProjectCommand.Object, mockTaskQuery.Object, mockValidator.Object);

            var taskId = Guid.NewGuid();
            var request = new TasksRequest
            {
                Name = "Updated Task",
                DueDate = DateTime.Now.AddDays(5),
                Status = 2,
                User = 3
            };

            var existingTask = new Domain.Entities.Task
            {
                TaskID = taskId,
                Name = "Old Task",
                DueDate = DateTime.Now.AddDays(1),
                Status = 1,
                AssignedTo = 1,
                ProjectID = Guid.NewGuid(),
                TaskStatus = new Domain.Entities.TaskStatus { Id = 1, Name = "Pending" },
                Users = new User { UserID = 1, Name = "User1", Email = "user1@gmail.com" }
            };

            var updatedTask = new Domain.Entities.Task
            {
                TaskID = taskId,
                Name = "Updated Task",
                DueDate = request.DueDate,
                Status = request.Status,
                AssignedTo = request.User,
                ProjectID = existingTask.ProjectID,
                TaskStatus = new Domain.Entities.TaskStatus { Id = request.Status, Name = "In Progress" },
                Users = new User { UserID = request.User, Name = "User2", Email = "user2@gmail.com" }
            };

            mockTaskQuery.Setup(q => q.GetTaskById(taskId)).ReturnsAsync(existingTask);
            mockProjectCommand.Setup(c => c.UpdateProjectTasks(It.IsAny<Domain.Entities.Task>())).Returns(System.Threading.Tasks.Task.CompletedTask);
            mockTaskQuery.Setup(q => q.GetTaskById(taskId)).ReturnsAsync(updatedTask); // Simulate retrieval of updated task

            // Act
            var result = await service.UpdateTask(taskId, request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Updated Task");
            result.DueDate.Should().Be(request.DueDate);
            result.Status.Should().NotBeNull();
            result.Status.Id.Should().Be(2);
            result.Status.Name.Should().Be("In Progress");
            result.UserAssigned.Should().NotBeNull();
            result.UserAssigned.Id.Should().Be(3);
            result.UserAssigned.Name.Should().Be("User2");
            result.UserAssigned.Email.Should().Be("user2@gmail.com");
        }
    }
}
