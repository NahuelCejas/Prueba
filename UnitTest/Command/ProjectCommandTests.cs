using Domain.Entities;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Command
{
    public class ProjectCommandTests
    {
        [Fact]
        public async System.Threading.Tasks.Task UpdateProject_ShouldUpdateProject_WhenProjectExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "UpdateProject_WhenProjectExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Existing Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10)
                };

                context.Projects.Add(project);
                await context.SaveChangesAsync();

                var service = new ProjectCommand(context);

                // Act
                project.ProjectName = "Updated Project";
                await service.UpdateProject(project);

                // Assert
                var updatedProject = await context.Projects.FindAsync(project.ProjectID);
                Assert.NotNull(updatedProject);
                Assert.Equal("Updated Project", updatedProject.ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProject_ShouldThrowException_WhenDbContextThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "UpdateProject_WhenDbContextThrowsException")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectCommand(context);
                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Existing Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10)
                };

                // Add the project to context but simulate exception on SaveChanges
                context.Projects.Add(project);
                await context.SaveChangesAsync();

                context.Dispose(); // Dispose context to simulate exception on the next operation

                // Act & Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() => service.UpdateProject(project));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task AddProjectInteractions_ShouldAddInteraction_WhenInteractionIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddProjectInteractions_WhenInteractionIsValid")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectCommand(context);

                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Project with Interaction",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10)
                };

                context.Projects.Add(project);
                await context.SaveChangesAsync();

                var interaction = new Interaction
                {
                    InteractionID = Guid.NewGuid(),
                    ProjectID = project.ProjectID,
                    Date = DateTime.Now,
                    Notes = "Initial Interaction"
                };

                // Act
                await service.AddProjectInteractions(interaction);

                // Assert
                var addedInteraction = await context.Interactions.FindAsync(interaction.InteractionID);
                Assert.NotNull(addedInteraction);
                Assert.Equal("Initial Interaction", addedInteraction.Notes);
                Assert.Equal(project.ProjectID, addedInteraction.ProjectID);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task AddProjectInteractions_ShouldThrowException_WhenDbContextThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddProjectInteractions_WhenDbContextThrowsException")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectCommand(context);

                var interaction = new Interaction
                {
                    InteractionID = Guid.NewGuid(),
                    ProjectID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Notes = "Test Interaction"
                };

                context.Dispose(); // Dispose context to simulate exception on the next operation

                // Act & Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() => service.AddProjectInteractions(interaction));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task AddProjectTasks_ShouldAddTask_WhenTaskIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddProjectTasks_WhenTaskIsValid")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectCommand(context);

                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Project with Task",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10)
                };

                context.Projects.Add(project);
                await context.SaveChangesAsync();

                var task = new Domain.Entities.Task
                {
                    TaskID = Guid.NewGuid(),
                    ProjectID = project.ProjectID,
                    Name = "Initial Task",
                    DueDate = DateTime.Now.AddDays(5)
                };

                // Act
                await service.AddProjectTasks(task);

                // Assert
                var addedTask = await context.Tasks.FindAsync(task.TaskID);
                Assert.NotNull(addedTask);
                Assert.Equal("Initial Task", addedTask.Name);
                Assert.Equal(project.ProjectID, addedTask.ProjectID);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task AddProjectTasks_ShouldThrowException_WhenDbContextThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "AddProjectTasks_WhenDbContextThrowsException")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectCommand(context);

                var task = new Domain.Entities.Task
                {
                    TaskID = Guid.NewGuid(),
                    ProjectID = Guid.NewGuid(),
                    Name = "Test Task",
                    DueDate = DateTime.Now.AddDays(5)
                };

                context.Dispose(); // Dispose context to simulate exception on the next operation

                // Act & Assert
                await Assert.ThrowsAsync<ObjectDisposedException>(() => service.AddProjectTasks(task));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task InsertProject_ShouldAddProjectToDatabase_WhenProjectIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usar una instancia única de base de datos en memoria
                .Options;

            using (var context = new AppDBContext(options))
            {
                var projectCommand = new ProjectCommand(context);
                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "New Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    ClientID = 1,
                    CampaignType = 1
                };

                // Act
                await projectCommand.InsertProject(project);

                // Assert
                var insertedProject = await context.Projects.FindAsync(project.ProjectID);
                Assert.NotNull(insertedProject);
                Assert.Equal("New Project", insertedProject.ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectTasks_ShouldUpdateTasksInDatabase_WhenTasksAreValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var projectCommand = new ProjectCommand(context);
                
                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Sample Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    ClientID = 1,
                    CampaignType = 1
                };

                var task = new Domain.Entities.Task
                {
                    TaskID = Guid.NewGuid(),
                    Name = "Initial Task",
                    DueDate = DateTime.Now.AddDays(5),
                    CreateDate = DateTime.Now,
                    ProjectID = project.ProjectID,
                    AssignedTo = 1,
                    Status = 1
                };

                context.Projects.Add(project);
                context.Tasks.Add(task);
                await context.SaveChangesAsync();

               
                task.Name = "Updated Task";
                task.DueDate = DateTime.Now.AddDays(7);

                // Act
                await projectCommand.UpdateProjectTasks(task);

                // Assert
                var updatedTask = await context.Tasks.FindAsync(task.TaskID);
                Assert.NotNull(updatedTask);
                Assert.Equal("Updated Task", updatedTask.Name);
                Assert.Equal(task.DueDate, updatedTask.DueDate);
            }
        }
    }
}
