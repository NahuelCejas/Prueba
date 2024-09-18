using Application.Interfaces.IQuery;
using Application.Response;
using Application.UseCase.CampaignTypesServices;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;


namespace UnitTest.UseCase.CampaignTypeServices
{
    public class CampaignTypeGetServicesTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnListOfGenericResponse_WhenQueryReturnsData()
        {
            // Arrange
            var mockQuery = new Mock<ICampaignTypeQuery>();
            var mockData = new List<CampaignType>
        {
            new CampaignType { Id = 1, Name = "SEO" },
            new CampaignType { Id = 2, Name = "PPC" }
        };

            mockQuery.Setup(q => q.GetListCampaignTypes()).ReturnsAsync(mockData);

            var service = new CampaignTypeGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("SEO");
            result[1].Name.Should().Be("PPC");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenQueryReturnsNoData()
        {
            // Arrange
            var mockQuery = new Mock<ICampaignTypeQuery>();
            mockQuery.Setup(q => q.GetListCampaignTypes()).ReturnsAsync(new List<CampaignType>());

            var service = new CampaignTypeGetServices(mockQuery.Object);

            // Act
            List<GenericResponse> result = await service.GetAll(); 

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
