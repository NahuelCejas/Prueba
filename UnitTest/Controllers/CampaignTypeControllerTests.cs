using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.IServices.ICampaignTypeServices;
using Application.Response;
using TP1_Rocio_Kreick.Controllers;

namespace UnitTest.Controllers
{
    public class CampaignTypeControllerTests
    {
        private readonly Mock<ICampaignTypeGetServices> _mockServices;
        private readonly CampaignTypeController _controller;

        public CampaignTypeControllerTests()
        {
            _mockServices = new Mock<ICampaignTypeGetServices>();
            _controller = new CampaignTypeController(_mockServices.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfCampaignTypes()
        {
            // Arrange
            var campaignTypes = new List<GenericResponse>
        {
            new GenericResponse { Id = 1, Name = "SEO" },
            new GenericResponse { Id = 2, Name = "PPC" }
        };

            _mockServices.Setup(service => service.GetAll()).ReturnsAsync(campaignTypes);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(campaignTypes);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoCampaignTypesExist()
        {
            // Arrange
            var emptyCampaignTypes = new List<GenericResponse>();

            _mockServices.Setup(service => service.GetAll()).ReturnsAsync(emptyCampaignTypes);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(emptyCampaignTypes);
        }
    }
}
