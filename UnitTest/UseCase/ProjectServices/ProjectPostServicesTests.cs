using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IValidator;
using Application.Request;
using Application.UseCase.ProjectServices;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UseCase.ProjectServices
{
    public class ProjectPostServicesTests
    {
        [Fact]
        public async System.Threading.Tasks.Task CreateProject_ShouldThrowValidationException_WhenRequestIsInvalid()
        {
            // Arrange
            var mockProjectQuery = new Mock<IProjectQuery>();
            var mockProjectCommand = new Mock<IProjectCommand>();
            var mockValidator = new Mock<IValidatorHandler<ProjectRequest>>();

            var service = new ProjectPostServices(mockProjectQuery.Object, mockProjectCommand.Object, mockValidator.Object);

            var request = new ProjectRequest();
            mockValidator.Setup(v => v.Validate(request)).ThrowsAsync(new ValidationException("Invalid request"));

            // Act
            Func<System.Threading.Tasks.Task> act = async () => { await service.CreateProject(request); };

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Invalid request");
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateProject_ShouldThrowNotFoundException_WhenProjectNotInserted()
        {
            // Arrange
            var mockProjectQuery = new Mock<IProjectQuery>();
            var mockProjectCommand = new Mock<IProjectCommand>();
            var mockValidator = new Mock<IValidatorHandler<ProjectRequest>>();

            var service = new ProjectPostServices(mockProjectQuery.Object, mockProjectCommand.Object, mockValidator.Object);

            var request = new ProjectRequest
            {
                Name = "New Project",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                Client = 1,
                CampaignType = 1
            };

            mockValidator.Setup(v => v.Validate(request)).Returns(System.Threading.Tasks.Task.CompletedTask);
            mockProjectCommand.Setup(c => c.InsertProject(It.IsAny<Domain.Entities.Project>())).Returns(System.Threading.Tasks.Task.CompletedTask);
            mockProjectQuery.Setup(q => q.GetProjectById(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Project)null);

            // Act
            Func<System.Threading.Tasks.Task> act = async () => { await service.CreateProject(request); };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Project not found");
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateProject_ShouldReturnProjectDetails_WhenProjectIsSuccessfullyCreated()
        {
            // Arrange
            var mockProjectQuery = new Mock<IProjectQuery>();
            var mockProjectCommand = new Mock<IProjectCommand>();
            var mockValidator = new Mock<IValidatorHandler<ProjectRequest>>();

            var service = new ProjectPostServices(mockProjectQuery.Object, mockProjectCommand.Object, mockValidator.Object);

            var request = new ProjectRequest
            {
                Name = "New Project",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(10),
                Client = 1,
                CampaignType = 1
            };

            var createdProjectId = Guid.NewGuid(); // Generate a new ID for the created project
            var createdProject = new Domain.Entities.Project
            {
                ProjectID = createdProjectId,
                ProjectName = request.Name,
                StartDate = request.Start,
                EndDate = request.End,
                ClientID = request.Client,
                CampaignType = request.CampaignType,
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street"
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "Campaign A"
                }
            };

            // Setup mock for validator
            mockValidator.Setup(v => v.Validate(request)).Returns(System.Threading.Tasks.Task.CompletedTask);

            // Setup mock for InsertProject to capture the inserted project and set its ID
            mockProjectCommand.Setup(c => c.InsertProject(It.IsAny<Domain.Entities.Project>()))
                .Callback<Domain.Entities.Project>(proj => proj.ProjectID = createdProjectId)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Setup mock for GetProjectById to return the created project
            mockProjectQuery.Setup(q => q.GetProjectById(createdProjectId)).ReturnsAsync(createdProject);

            // Act
            var result = await service.CreateProject(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Name.Should().Be("New Project");
            result.Data.Client.Should().NotBeNull();
            result.Data.Client.Name.Should().Be("Client1");
            result.Data.CampaignType.Should().NotBeNull();
            result.Data.CampaignType.Name.Should().Be("Campaign A");
        }
    }
}
