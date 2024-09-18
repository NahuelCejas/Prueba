using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Query;
using Infrastructure.Persistence;
using Domain.Entities;
using System;
using System.Threading.Tasks;
namespace UnitTest.Query
{
    public class ClientQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ClientExists_ShouldReturnTrue_WhenClientExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "ClientExists_WhenClientExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ClientQuery(context);

                var client = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street"
                };

                context.Clients.Add(client);
                await context.SaveChangesAsync();

                // Act
                bool result = await service.ClientExists(client.ClientID);

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task ClientExists_ShouldReturnFalse_WhenClientDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "ClientExists_WhenClientDoesNotExist")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ClientQuery(context);

                // Act
                bool result = await service.ClientExists(999); // Using an ID that does not exist

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListClients_ShouldReturnAllClients_WhenTheyExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                // Insertar datos de prueba
                context.Clients.AddRange(
                    new Client { ClientID = 1, Name = "Client1", Email = "client1@gmail.com", Phone = "123456789", Company = "Company A", Address = "123 Street", CreateDate = DateTime.Now },
                    new Client { ClientID = 2, Name = "Client2", Email = "client2@gmail.com", Phone = "987654321", Company = "Company B", Address = "456 Avenue", CreateDate = DateTime.Now }
                );
                await context.SaveChangesAsync();

                var query = new ClientQuery(context);

                // Act
                var result = await query.GetListClients();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Contains(result, c => c.Name == "Client1");
                Assert.Contains(result, c => c.Name == "Client2");
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListClients_ShouldReturnEmptyList_WhenNoClientsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var query = new ClientQuery(context);

                // Act
                var result = await query.GetListClients();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}
