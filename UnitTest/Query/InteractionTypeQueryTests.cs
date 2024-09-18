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
    public class InteractionTypeQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetInteractionTypeById_ShouldReturnInteractionType_WhenInteractionTypeExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetInteractionTypeById_WhenInteractionTypeExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new InteractionTypeQuery(context);

                var interactionType = new InteractionType
                {
                    Id = 1,
                    Name = "Initial Meeting"
                };

                context.InteractionTypes.Add(interactionType);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetInteractionTypeById(interactionType.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(interactionType.Id, result.Id);
                Assert.Equal(interactionType.Name, result.Name);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetInteractionTypeById_ShouldThrowNotFoundException_WhenInteractionTypeDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetInteractionTypeById_WhenInteractionTypeDoesNotExist")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new InteractionTypeQuery(context);
                var nonExistentId = 999; // Use an ID that does not exist

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetInteractionTypeById(nonExistentId));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListInteractionTypes_ShouldReturnAllInteractionTypes_WhenTheyExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                // Insertar datos de prueba
                context.InteractionTypes.AddRange(
                    new InteractionType { Id = 1, Name = "Initial Meeting" },
                    new InteractionType { Id = 2, Name = "Phone Call" }
                );
                await context.SaveChangesAsync();

                var query = new InteractionTypeQuery(context);

                // Act
                var result = await query.GetListInteractionTypes();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Contains(result, it => it.Name == "Initial Meeting");
                Assert.Contains(result, it => it.Name == "Phone Call");
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListInteractionTypes_ShouldReturnEmptyList_WhenNoInteractionTypesExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var query = new InteractionTypeQuery(context);

                // Act
                var result = await query.GetListInteractionTypes();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}
