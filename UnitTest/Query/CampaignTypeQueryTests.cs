using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Query
{
    public class CampaignTypeQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetListCampaignTypes_ShouldReturnAllCampaignTypes_WhenTheyExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                
                context.CampaignTypes.AddRange(
                    new CampaignType { Id = 1, Name = "SEO" },
                    new CampaignType { Id = 2, Name = "PPC" }
                );
                await context.SaveChangesAsync();

                var query = new CampaignTypeQuery(context);

                // Act
                var result = await query.GetListCampaignTypes();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Contains(result, ct => ct.Name == "SEO");
                Assert.Contains(result, ct => ct.Name == "PPC");
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetListCampaignTypes_ShouldReturnEmptyList_WhenNoCampaignTypesExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var query = new CampaignTypeQuery(context);

                // Act
                var result = await query.GetListCampaignTypes();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}
