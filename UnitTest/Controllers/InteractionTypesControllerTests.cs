using Application.Interfaces.IServices.IInteractionTypeServices;
using Application.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1_Rocio_Kreick.Controllers;

namespace UnitTest.Controllers
{
    public class InteractionTypesControllerTests
    {
        private readonly Mock<IInteractionTypeGetServices> _mockService;
        private readonly InteractionTypesController _controller;

        public InteractionTypesControllerTests()
        {
            _mockService = new Mock<IInteractionTypeGetServices>();
            _controller = new InteractionTypesController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfGenericResponse()
        {
            // Arrange
            var interactionTypes = new List<GenericResponse>
        {
            new GenericResponse { Id = 1, Name = "Initial Meeting" },
            new GenericResponse { Id = 2, Name = "Phone Call" }
        };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(interactionTypes);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(200);
            var returnValue = jsonResult.Value.Should().BeAssignableTo<IEnumerable<GenericResponse>>().Subject;
            returnValue.Should().BeEquivalentTo(interactionTypes);
        }
    }
}
