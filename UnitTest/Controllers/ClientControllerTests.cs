using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.IServices.IClientServices;
using Application.Response;
using Application.Models;
using Application.Exceptions;
using TP1_Rocio_Kreick.Controllers;
using Microsoft.IdentityModel.SecurityTokenService;

namespace UnitTest.Controllers
{
    public class ClientControllerTests
    {
        private readonly Mock<IClientGetServices> _mockGetService;
        private readonly Mock<IClientPostServices> _mockPostService;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _mockGetService = new Mock<IClientGetServices>();
            _mockPostService = new Mock<IClientPostServices>();
            _controller = new ClientController(_mockGetService.Object, _mockPostService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfClients()
        {
            // Arrange
            var clients = new List<Clients>
        {
            new Clients { Id = 1, Name = "Client A", Email = "clienta@example.com" },
            new Clients { Id = 2, Name = "Client B", Email = "clientb@example.com" }
        };

            _mockGetService.Setup(service => service.GetAll()).ReturnsAsync(clients);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(clients);
        }

        [Fact]
        public async Task CreateClient_ShouldReturnCreatedResult_WhenClientIsCreated()
        {
            // Arrange
            var clientRequest = new ClientsRequest
            {
                Name = "Client 1",
                Email = "client1@gmail.com",
                Phone = "123456789",
                Company = "Company A",
                Address = "123 Main St"
            };

            var clientResponse = new Clients
            {
                Id = 1,
                Name = clientRequest.Name,
                Email = clientRequest.Email
            };

            _mockPostService.Setup(service => service.CreateClient(clientRequest)).ReturnsAsync(clientResponse);

            // Act
            var result = await _controller.CreateClient(clientRequest);

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(201); 
            jsonResult.Value.Should().BeEquivalentTo(clientResponse); 
        }

        [Fact]
        public async Task CreateClient_ShouldReturnBadRequest_WhenClientCreationFails()
        {
            // Arrange
            var clientRequest = new ClientsRequest
            {
                Name = "", 
                Email = "invalid-email", 
                Phone = "123",
                Company = "Company A",
                Address = ""
            };

            var validationFailure = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Name", "Name is required."),
                new FluentValidation.Results.ValidationFailure("Email", "Invalid email format.")
            };

            _mockPostService.Setup(service => service.CreateClient(clientRequest))
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailure));

            // Act
            var result = await _controller.CreateClient(clientRequest);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().BeEquivalentTo(validationFailure); 
        }
    }
}
