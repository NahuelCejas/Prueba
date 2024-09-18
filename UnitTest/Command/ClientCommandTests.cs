using Domain.Entities;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Command
{
    public class ClientCommandTests
    {
        [Fact]
        public async System.Threading.Tasks.Task InsertClient_ShouldAddClientToDatabase_WhenClientIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usar una instancia única de base de datos en memoria
                .Options;

            using (var context = new AppDBContext(options))
            {
                var clientCommand = new ClientCommand(context);
                var client = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Phone = "123456789",
                    Company = "Company A",
                    Address = "123 Main St",
                    CreateDate = DateTime.Now
                };

                // Act
                await clientCommand.InsertClient(client);

                // Assert
                var insertedClient = await context.Clients.FindAsync(client.ClientID);
                Assert.NotNull(insertedClient);
                Assert.Equal("Client1", insertedClient.Name);
                Assert.Equal("client1@gmail.com", insertedClient.Email);
                Assert.Equal("123456789", insertedClient.Phone);
                Assert.Equal("Company A", insertedClient.Company);
                Assert.Equal("123 Main St", insertedClient.Address);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task InsertClient_ShouldThrowException_WhenClientIsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usar una instancia única de base de datos en memoria
                .Options;

            using (var context = new AppDBContext(options))
            {
                var clientCommand = new ClientCommand(context);

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() => clientCommand.InsertClient(null));
            }
        }
    }
}
