using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Query;
using Infrastructure.Persistence;
using Domain.Entities;
using Application.Exceptions;
using System;
using System.Threading.Tasks;
namespace UnitTest.Query
{
    public class InteractionQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetInteractionById_ShouldReturnInteraction_WhenInteractionExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetInteractionById_WhenInteractionExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new InteractionQuery(context);

                var interactionType = new InteractionType
                {
                    Id = 1,
                    Name = "Initial Meeting"
                };

                var interaction = new Interaction
                {
                    InteractionID = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Notes = "Follow-up call",
                    InteractionType = interactionType.Id,
                    InteractionTypes = interactionType
                };

                context.Interactions.Add(interaction);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetInteractionById(interaction.InteractionID);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(interaction.InteractionID, result.InteractionID);
                Assert.Equal(interaction.Notes, result.Notes);
                Assert.Equal(interaction.InteractionTypes.Id, result.InteractionTypes.Id);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetInteractionById_ShouldThrowNotFoundException_WhenInteractionDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetInteractionById_WhenInteractionDoesNotExist")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new InteractionQuery(context);
                var nonExistentId = Guid.NewGuid();

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetInteractionById(nonExistentId));
            }
        }
    }
}
