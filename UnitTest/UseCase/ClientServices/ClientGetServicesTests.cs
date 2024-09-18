using Xunit;
using Moq;
using FluentAssertions;
using Application.UseCase.ClientServices;
using Application.Interfaces.IQuery;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Application.Response;

namespace UnitTest.UseCase.ClientServices
{
    public class ClientGetServicesTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnListOfClientsResponse_WhenQueryReturnsData()
        {
            // Arrange
            var mockQuery = new Mock<IClientQuery>();
            var mockData = new List<Client>
            {
                new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@example.com",
                    Phone = "1155468754",
                    Company = "Company1",
                    Address = "Address1",
                    CreateDate = DateTime.Now,
                },
                new Client
                {
                    ClientID = 2,
                    Name = "Client2",
                    Email = "client2@example.com",
                    Phone = "1144678458",
                    Company = "Company2",
                    Address = "Address2",
                    CreateDate = DateTime.Now,
                }
            };

            mockQuery.Setup(q => q.GetListClients()).ReturnsAsync(mockData);

            var service = new ClientGetServices(mockQuery.Object);

            // Act
            List<Clients> result = await service.GetAll(); // Should return a list of 'Clients' response

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].Name.Should().Be("Client1");
            result[0].Email.Should().Be("client1@example.com");
            result[0].Phone.Should().Be("1155468754");
            result[0].Company.Should().Be("Company1");
            result[0].Address.Should().Be("Address1");

            result[1].Id.Should().Be(2);
            result[1].Name.Should().Be("Client2");
            result[1].Email.Should().Be("client2@example.com");
            result[1].Phone.Should().Be("1144678458");
            result[1].Company.Should().Be("Company2");
            result[1].Address.Should().Be("Address2");
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenQueryReturnsNoData()
        {
            // Arrange
            var mockQuery = new Mock<IClientQuery>();
            mockQuery.Setup(q => q.GetListClients()).ReturnsAsync(new List<Client>());

            var service = new ClientGetServices(mockQuery.Object);

            // Act
            List<Clients> result = await service.GetAll(); // Should return an empty list 

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}