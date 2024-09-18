using Application.Exceptions;
using Application.Interfaces.IServices.IProjectServices;
using Application.Models;
using Application.Request;
using Application.Response;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TP1_Rocio_Kreick.Controllers;
using Xunit;

namespace UnitTest.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IProjectPatchServices> _mockProjectPatchService;        
        private readonly Mock<IProjectPutServices> _mockProjectPutService;
        private readonly Mock<IProjectGetServices> _mockProjectGetService;
        private readonly Mock<IProjectPostServices> _mockProjectPostService;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            _mockProjectPatchService = new Mock<IProjectPatchServices>();
            _mockProjectPutService = new Mock<IProjectPutServices>();
            _mockProjectGetService = new Mock<IProjectGetServices>();
            _mockProjectPostService = new Mock<IProjectPostServices>();
            _controller = new ProjectController(_mockProjectGetService.Object, _mockProjectPostService.Object, _mockProjectPatchService.Object, _mockProjectPutService.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnOkResult_WhenTaskIsAddedSuccessfully()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskRequest = new TasksRequest { Name = "New Task", DueDate = DateTime.Now.AddDays(5) };
            var taskResponse = new Tasks { Id = Guid.NewGuid(), Name = "New Task", DueDate = DateTime.Now.AddDays(5) };

            _mockProjectPatchService.Setup(service => service.AddTask(projectId, taskRequest)).ReturnsAsync(taskResponse);

            // Act
            var result = await _controller.AddTask(projectId, taskRequest);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(201);
            jsonResult.Value.Should().BeEquivalentTo(taskResponse);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnBadRequest_WhenValidationExceptionIsThrown()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskRequest = new TasksRequest { Name = "", DueDate = DateTime.Now.AddDays(-1) };

            _mockProjectPatchService.Setup(service => service.AddTask(projectId, taskRequest))
                .ThrowsAsync(new ValidationException("Invalid data"));

            // Act
            var result = await _controller.AddTask(projectId, taskRequest);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldReturnOkResult_WhenTaskIsUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var taskRequest = new TasksRequest
            {
                Name = "New Task",
                DueDate = DateTime.Now,
                User = 1,
                Status = 1
            };

            var taskResponse = new Tasks
            {
                Id = id,
                Name = taskRequest.Name,
                DueDate = taskRequest.DueDate,
                ProjectId = Guid.NewGuid(),
                Status = new GenericResponse { Id = 1, Name = "Pending" },
                UserAssigned = new Users { Id = 1, Name = "User 1", Email = "user1@gmail.com" }
            };

            _mockProjectPutService.Setup(service => service.UpdateTask(id, taskRequest)).ReturnsAsync(taskResponse);

            // Act
            var result = await _controller.UpdateTask(id, taskRequest);

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(taskResponse);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnProjectsList_WhenValidFiltersAreApplied()
        {
            // Arrange
            var projectResponseList = new List<Application.Response.Project>
            {
                new Application.Response.Project { Id = Guid.NewGuid(), Name = "Project A", Start = DateTime.Now, End = DateTime.Now.AddMonths(1) },
                new Application.Response.Project { Id = Guid.NewGuid(), Name = "Project B", Start = DateTime.Now, End = DateTime.Now.AddMonths(2) }
            };

            _mockProjectGetService.Setup(service => service.GetProjects(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(projectResponseList);

            // Act
            var result = await _controller.GetProjects("Project", null, null, 0, 2);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(200);
            jsonResult.Value.Should().BeEquivalentTo(projectResponseList);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateProject_ShouldReturnCreatedResult_WhenProjectIsCreated()
        {
            // Arrange
            var projectRequest = new ProjectRequest
            {
                Name = "New Project",
                Start = DateTime.Now,
                End = DateTime.Now.AddMonths(1),
                Client = 1,
                CampaignType = 2
            };
            
            var clientResponse = new Clients
            {
                Id = projectRequest.Client,
                Name = "Client 1",
                Phone = "123456789",
                Company = "Company A",
                Address = "123 Street",
                Email = "client1@gmail.com"
            };

            var campaignTypeResponse = new GenericResponse
            {
                Id = projectRequest.CampaignType,
                Name = "SEO"
            };
                        
            var interaction1 = new Interactions
            {
                Id = Guid.NewGuid(),
                Notes = "First interaction",
                Date = DateTime.Now,
                ProjectId = Guid.NewGuid(),
                InteractionType = new GenericResponse { Id = 1, Name = "Initial Meeting" }
            };

            var interaction2 = new Interactions
            {
                Id = Guid.NewGuid(),
                Notes = "Second interaction",
                Date = DateTime.Now,
                ProjectId = Guid.NewGuid(),
                InteractionType = new GenericResponse { Id = 2, Name = "Phone Call" }
            };
            
            var task1 = new Tasks
            {
                Id = Guid.NewGuid(),
                Name = "Task 1",
                DueDate = DateTime.Now.AddDays(10),
                ProjectId = Guid.NewGuid(),
                Status = new GenericResponse { Id = 1, Name = "Pending" },
                UserAssigned = new Users { Id = 1, Name = "User 1", Email = "user1@egmail.com" }
            };

            var task2 = new Tasks
            {
                Id = Guid.NewGuid(),
                Name = "Task 2",
                DueDate = DateTime.Now.AddDays(20),
                ProjectId = Guid.NewGuid(),
                Status = new GenericResponse { Id = 2, Name = "In Progress" },
                UserAssigned = new Users { Id = 2, Name = "User 2", Email = "user2@gmail.com" }
            };
            
            var projectDetailsResponse = new ProjectDetails
            {
                Data = new Application.Response.Project
                {
                    Id = Guid.NewGuid(),
                    Name = projectRequest.Name,
                    Start = projectRequest.Start,
                    End = projectRequest.End,
                    Client = clientResponse,
                    CampaignType = campaignTypeResponse
                },
                Interactions = new List<Interactions> { interaction1, interaction2 },
                Tasks = new List<Tasks> { task1, task2 }
            };
            
            _mockProjectPostService.Setup(service => service.CreateProject(projectRequest)).ReturnsAsync(projectDetailsResponse);

            // Act
            var result = await _controller.CreateProject(projectRequest);

            // Assert
            var createdResult = result.Should().BeOfType<JsonResult>().Subject;
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(projectDetailsResponse);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            
            var projectEntity = new Domain.Entities.Project
            {
                ProjectID = projectId,
                ProjectName = "Existing Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CreateDate = DateTime.Now.AddMonths(-1),
                UpdateDate = null,
                ClientID = 1,
                CampaignType = 1,
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client 1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Main St",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "SEO"
                },
                ListTasks = new List<Domain.Entities.Task>
                {
                    new Domain.Entities.Task { TaskID = Guid.NewGuid(), Name = "Task 1", DueDate = DateTime.Now.AddDays(5), AssignedTo = 1, Status = 1 },
                    new Domain.Entities.Task { TaskID = Guid.NewGuid(), Name = "Task 2", DueDate = DateTime.Now.AddDays(10), AssignedTo = 2, Status = 2 }
                },
                ListInteractions = new List<Interaction>
                {
                    new Interaction { InteractionID = Guid.NewGuid(), Date = DateTime.Now, Notes = "Interaction 1", InteractionType = 1 },
                    new Interaction { InteractionID = Guid.NewGuid(), Date = DateTime.Now.AddDays(-1), Notes = "Interaction 2", InteractionType = 2 }
                }
            };
            
            var projectDetailsResponse = new ProjectDetails
            {
                Data = new Application.Response.Project
                {
                    Id = projectEntity.ProjectID,
                    Name = projectEntity.ProjectName,
                    Start = projectEntity.StartDate,
                    End = projectEntity.EndDate,
                    Client = new Clients
                    {
                        Id = projectEntity.Clients.ClientID,
                        Name = projectEntity.Clients.Name,
                        Phone = projectEntity.Clients.Phone,
                        Company = projectEntity.Clients.Company,
                        Address = projectEntity.Clients.Address,
                        Email = projectEntity.Clients.Email
                    },
                    CampaignType = new GenericResponse
                    {
                        Id = projectEntity.CampaignTypes.Id,
                        Name = projectEntity.CampaignTypes.Name
                    }
                },
                Interactions = projectEntity.ListInteractions.Select(i => new Interactions
                {
                    Id = i.InteractionID,
                    Date = i.Date,
                    Notes = i.Notes,
                    ProjectId = i.ProjectID,
                    InteractionType = new GenericResponse { Id = i.InteractionType, Name = "TypeName" }  
                }).ToList(),
                Tasks = projectEntity.ListTasks.Select(t => new Tasks
                {
                    Id = t.TaskID,
                    Name = t.Name,
                    DueDate = t.DueDate,
                    ProjectId = t.ProjectID,
                    Status = new GenericResponse { Id = t.Status, Name = "StatusName" },  
                    UserAssigned = new Users { Id = t.AssignedTo, Name = "User Name", Email = "user@example.com" } 
                }).ToList()
            };
            
            _mockProjectGetService.Setup(service => service.GetProjectById(projectId)).ReturnsAsync(projectDetailsResponse);

            // Act
            var result = await _controller.GetProjectById(projectId);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(200);
            jsonResult.Value.Should().BeEquivalentTo(projectDetailsResponse);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _mockProjectGetService.Setup(service => service.GetProjectById(projectId))
                .ThrowsAsync(new NotFoundException("Project not found"));

            // Act
            var result = await _controller.GetProjectById(projectId);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(404);
            jsonResult.Value.Should().BeEquivalentTo(new ApiError { Message = "Project not found" });
        }

        [Fact]
        public async System.Threading.Tasks.Task AddInteraction_ShouldReturnCreatedResult_WhenInteractionIsAddedSuccessfully()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var interactionRequest = new InteractionsRequest
            {
                InteractionType = 1,
                Notes = "Interaction notes",
                Date = DateTime.Now 
            };

            var interactionResponse = new Interactions
            {
                Id = Guid.NewGuid(),
                Notes = interactionRequest.Notes,
                Date = interactionRequest.Date,
                ProjectId = projectId,
                InteractionType = new GenericResponse { Id = 1, Name = "Initial Meeting" }
            };

            _mockProjectPatchService.Setup(service => service.AddInteraction(projectId, interactionRequest)).ReturnsAsync(interactionResponse);

            // Act
            var result = await _controller.AddInteraction(projectId, interactionRequest);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(201);
            jsonResult.Value.Should().BeEquivalentTo(interactionResponse);
        }
    }
}

