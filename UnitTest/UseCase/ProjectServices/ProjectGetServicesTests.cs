using Xunit;
using Moq;
using FluentAssertions;
using Application.UseCase.ProjectServices;
using Application.Interfaces.IQuery;
using Application.Response;
using Application.Exceptions;
using Domain.Entities;
using System;
using System.Threading.Tasks;


namespace UnitTest.UseCase.ProjectServices
{
    public class ProjectGetServicesTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnProjectDetails_WhenProjectExists()
        {
            // Arrange
            var mockQuery = new Mock<IProjectQuery>();
            var projectId = Guid.NewGuid();

            var clientEntity = new Client { ClientID = 1, Name = "Client1" };
            var campaignTypeEntity = new CampaignType { Id = 2, Name = "Campaign1" };

            var mockProject = new Domain.Entities.Project
            {
                ProjectID = projectId,
                ProjectName = "Project 1",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(10),
                CreateDate = DateTime.Now.AddDays(-15),
                UpdateDate = DateTime.Now.AddDays(-5),
                ClientID = 1,
                CampaignType = 2,
                Clients = clientEntity,
                CampaignTypes = campaignTypeEntity,
                ListTasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task { TaskID = Guid.NewGuid(), Name = "Task 1", DueDate = DateTime.Now.AddDays(5), CreateDate = DateTime.Now.AddDays(-10), UpdateDate = DateTime.Now.AddDays(-2), ProjectID = projectId, AssignedTo = 1, Status = 1, Users = new User { UserID = 1, Name = "User1", Email = "user1@example.com" }, TaskStatus = new Domain.Entities.TaskStatus { Id = 1, Name = "Pending" } },
                new Domain.Entities.Task { TaskID = Guid.NewGuid(), Name = "Task 2", DueDate = DateTime.Now.AddDays(10), CreateDate = DateTime.Now.AddDays(-8), UpdateDate = DateTime.Now.AddDays(-1), ProjectID = projectId, AssignedTo = 2, Status = 2, Users = new User { UserID = 2, Name = "User2", Email = "user2@example.com" }, TaskStatus = new Domain.Entities.TaskStatus { Id = 2, Name = "In Progress" } }
            },
                ListInteractions = new List<Interaction>
            {
                new Interaction { InteractionID = Guid.NewGuid(), Date = DateTime.Now.AddDays(-2), Notes = "Initial contact", ProjectID = projectId, InteractionType = 1, InteractionTypes = new InteractionType { Id = 1, Name = "Initial Meeting" } },
                new Interaction { InteractionID = Guid.NewGuid(), Date = DateTime.Now.AddDays(-1), Notes = "Follow-up call", ProjectID = projectId, InteractionType = 2, InteractionTypes = new InteractionType { Id = 2, Name = "Phone Call" } }
            }
            };
            
            var interactionsList = mockProject.ListInteractions.ToList();
            var tasksList = mockProject.ListTasks.ToList();
            
            var mappedProjectResponse = new ProjectDetails
            {
                Data = new Application.Response.Project
                {
                    Id = mockProject.ProjectID,
                    Name = mockProject.ProjectName,
                    Start = mockProject.StartDate,
                    End = mockProject.EndDate,
                    Client = new Clients
                    {
                        Id = clientEntity.ClientID,
                        Name = clientEntity.Name
                    },
                    CampaignType = new GenericResponse
                    {
                        Id = campaignTypeEntity.Id,
                        Name = campaignTypeEntity.Name
                    }
                },
                Interactions = new List<Interactions>
            {
                new Interactions
                {
                    Id = interactionsList[0].InteractionID,
                    Date = interactionsList[0].Date,
                    Notes = interactionsList[0].Notes,
                    ProjectId = interactionsList[0].ProjectID,
                    InteractionType = new GenericResponse
                    {
                        Id = interactionsList[0].InteractionTypes.Id,
                        Name = interactionsList[0].InteractionTypes.Name
                    }
                },
                new Interactions
                {
                    Id = interactionsList[1].InteractionID,
                    Date = interactionsList[1].Date,
                    Notes = interactionsList[1].Notes,
                    ProjectId = interactionsList[1].ProjectID,
                    InteractionType = new GenericResponse
                    {
                        Id = interactionsList[1].InteractionTypes.Id,
                        Name = interactionsList[1].InteractionTypes.Name
                    }
                }
            },
                Tasks = new List<Tasks>
            {
                new Tasks
                {
                    Id = tasksList[0].TaskID,
                    Name = tasksList[0].Name,
                    DueDate = tasksList[0].DueDate,
                    ProjectId = tasksList[0].ProjectID,
                    Status = new GenericResponse
                    {
                        Id = tasksList[0].TaskStatus.Id,
                        Name = tasksList[0].TaskStatus.Name
                    },
                    UserAssigned = new Users
                    {
                        Id = tasksList[0].Users.UserID,
                        Name = tasksList[0].Users.Name,
                        Email = tasksList[0].Users.Email
                    }
                },
                new Tasks
                {
                    Id = tasksList[1].TaskID,
                    Name = tasksList[1].Name,
                    DueDate = tasksList[1].DueDate,
                    ProjectId = tasksList[1].ProjectID,
                    Status = new GenericResponse
                    {
                        Id = tasksList[1].TaskStatus.Id,
                        Name = tasksList[1].TaskStatus.Name
                    },
                    UserAssigned = new Users
                    {
                        Id = tasksList[1].Users.UserID,
                        Name = tasksList[1].Users.Name,
                        Email = tasksList[1].Users.Email
                    }
                }
            }
            };

            mockQuery.Setup(q => q.GetProjectById(projectId)).ReturnsAsync(mockProject);

            var service = new ProjectGetServices(mockQuery.Object);

            // Act
            var result = await service.GetProjectById(projectId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(projectId);
            result.Data.Name.Should().Be("Project 1");
            result.Data.Client.Name.Should().Be("Client1");
            result.Data.CampaignType.Name.Should().Be("Campaign1");
            result.Interactions.Should().NotBeNull().And.HaveCount(2); 
            result.Tasks.Should().NotBeNull().And.HaveCount(2); 
            result.Tasks[0].Name.Should().Be("Task 1");
            result.Interactions[1].Notes.Should().Be("Follow-up call");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var mockQuery = new Mock<IProjectQuery>();
            var projectId = Guid.NewGuid();

            mockQuery.Setup(q => q.GetProjectById(projectId)).ReturnsAsync((Domain.Entities.Project)null);

            var service = new ProjectGetServices(mockQuery.Object);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => { await service.GetProjectById(projectId); };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Project not found");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnEmptyList_WhenNoProjectsMatch()
        {
            // Arrange
            var mockProjectQuery = new Mock<IProjectQuery>();
            var service = new ProjectGetServices(mockProjectQuery.Object);

            mockProjectQuery.Setup(q => q.GetProjects(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new List<Domain.Entities.Project>());

            // Act
            var result = await service.GetProjects(null, null, null, null, null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnListOfProjects_WhenProjectsMatch()
        {
            // Arrange
            var mockProjectQuery = new Mock<IProjectQuery>();
            var service = new ProjectGetServices(mockProjectQuery.Object);

            var projects = new List<Domain.Entities.Project>
        {
            new Domain.Entities.Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project A",
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(10),
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client A",
                    Email = "clienta@example.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street"
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "Campaign A"
                }
            },
            new Domain.Entities.Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project B",
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(5),
                Clients = null,
                CampaignTypes = null
            }
        };

            mockProjectQuery.Setup(q => q.GetProjects(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(projects);

            // Act
            var result = await service.GetProjects(null, null, null, null, null);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].Name.Should().Be("Project A");
            result[0].Client.Should().NotBeNull();
            result[0].Client.Name.Should().Be("Client A");
            result[0].CampaignType.Should().NotBeNull();
            result[0].CampaignType.Name.Should().Be("Campaign A");

            result[1].Name.Should().Be("Project B");
            result[1].Client.Should().BeNull();
            result[1].CampaignType.Should().BeNull();
        }
    }
}
