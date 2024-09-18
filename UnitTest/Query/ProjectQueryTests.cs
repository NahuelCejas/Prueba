using Application.Exceptions;
using Infrastructure.Persistence;
using Infrastructure.Query;
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
    public class ProjectQueryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetProjectByName_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectByName_WhenProjectExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Test Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    ClientID = 1,
                    CampaignType = 1
                };

                context.Projects.Add(project);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetProjectByName("Test Project");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(project.ProjectName, result.ProjectName);
                Assert.Equal(project.ProjectID, result.ProjectID);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectByName_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectByName_WhenProjectDoesNotExist")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);
                var nonExistentName = "NonExistent Project";

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetProjectByName(nonExistentName));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectById_WhenProjectExists")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var project = new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Existing Project",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    ClientID = 1,
                    CampaignType = 1,
                    Clients = new Client
                    {
                        ClientID = 1,
                        Name = "Client1",
                        Email = "client1@gmail.com",
                        Company = "Company A",
                        Phone = "123456789",
                        Address = "123 Street"
                    },
                    CampaignTypes = new CampaignType
                    {
                        Id = 1,
                        Name = "SEO"
                    }
                };

                context.Projects.Add(project);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetProjectById(project.ProjectID);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(project.ProjectID, result.ProjectID);
                Assert.Equal(project.ProjectName, result.ProjectName);
                Assert.Equal(project.Clients.ClientID, result.Clients.ClientID);
                Assert.Equal(project.CampaignTypes.Id, result.CampaignTypes.Id);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectById_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "GetProjectById_WhenProjectDoesNotExist")
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);
                var nonExistentId = Guid.NewGuid();

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetProjectById(nonExistentId));
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnAllProjects_WhenNoFiltersAreApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var projects = new List<Project>
            {
                new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Project A",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    ClientID = 1,
                    CampaignType = 1,
                    Clients = new Client
                    {
                        ClientID = 1,
                        Name = "Client1",
                        Email = "client1@gmail.com",
                        Company = "Company A",
                        Phone = "123456789",
                        Address = "123 Street",
                        CreateDate = DateTime.Now
                    },
                    CampaignTypes = new CampaignType
                    {
                        Id = 1,
                        Name = "SEO"
                    }
                },
                new Project
                {
                    ProjectID = Guid.NewGuid(),
                    ProjectName = "Project B",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(20),
                    ClientID = 2,
                    CampaignType = 2,
                    Clients = new Client
                    {
                        ClientID = 2,
                        Name = "Client2",
                        Email = "client2@gmail.com",
                        Company = "Company B",
                        Phone = "987654321",
                        Address = "456 Avenue",
                        CreateDate = DateTime.Now
                    },
                    CampaignTypes = new CampaignType
                    {
                        Id = 2,
                        Name = "PPC"
                    }
                }
            };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

               
                var count = await context.Projects.CountAsync();
                Assert.Equal(2, count); 

                
                var allProjects = await context.Projects.Include(p => p.Clients).Include(p => p.CampaignTypes).ToListAsync();
                foreach (var proj in allProjects)
                {
                    Console.WriteLine($"Project: {proj.ProjectName}, Client: {proj.Clients.Name}, Campaign: {proj.CampaignTypes.Name}");
                }

                // Act
                var result = (await service.GetProjects(null, null, null, null, null)).ToList();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnFilteredProjects_WhenNameFilterIsApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var projects = new List<Project>
        {
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project A",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ClientID = 1,
                CampaignType = 1,
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "SEO"
                }
            },
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project B",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                ClientID = 2,
                CampaignType = 2,
                Clients = new Client
                {
                    ClientID = 2,
                    Name = "Client2",
                    Email = "client2@gmail.com",
                    Company = "Company B",
                    Phone = "987654321",
                    Address = "456 Avenue",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 2,
                    Name = "PPC"
                }
            }
        };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                // Verify that the data has been entered correctly
                List<Project> allProjects = await context.Projects.Include(p => p.Clients).Include(p => p.CampaignTypes).ToListAsync();
                foreach (var proj in allProjects)
                {
                    Console.WriteLine($"Project: {proj.ProjectName}, Client: {proj.Clients.Name}, Campaign: {proj.CampaignTypes.Name}");
                }

                // Act
                var result = (await service.GetProjects("Project A", null, null, null, null)).ToList(); 

                // Assert
                Assert.NotNull(result);
                Assert.Single(result); 
                Assert.Equal("Project A", result[0].ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnPagedProjects_WhenPaginationIsApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                for (int i = 1; i <= 10; i++)
                {
                    context.Projects.Add(new Project
                    {
                        ProjectID = Guid.NewGuid(),
                        ProjectName = $"Project {i}",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10 + i),
                        ClientID = i,
                        CampaignType = i,
                        Clients = new Client
                        {
                            ClientID = i,
                            Name = $"Client {i}",
                            Email = $"client{i}@gmail.com",
                            Company = $"Company {i}",
                            Phone = $"{i}23456789",
                            Address = $"{i} Street",
                            CreateDate = DateTime.Now
                        },
                        CampaignTypes = new CampaignType
                        {
                            Id = i,
                            Name = $"Campaign {i}"
                        }
                    });
                }

                await context.SaveChangesAsync();

                // Verify that the data has been entered correctly
                var totalCount = await context.Projects.CountAsync();
                Assert.Equal(10, totalCount); // Ensures that all projects have been inserted correctly

                // Debugging: List the names of the projects in the database
                List<Project> allProjects = await context.Projects.Include(p => p.Clients).Include(p => p.CampaignTypes).ToListAsync();
                foreach (var proj in allProjects)
                {
                    Console.WriteLine($"Project: {proj.ProjectName}, Client: {proj.Clients.Name}, Campaign: {proj.CampaignTypes.Name}");
                }

                // Act
                var result = (await service.GetProjects(null, null, null, 2, 3)).ToList(); 

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Equal("Project 3", result[0].ProjectName);
                Assert.Equal("Project 4", result[1].ProjectName);
                Assert.Equal("Project 5", result[2].ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnFilteredProjects_WhenCampaignFilterIsApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var projects = new List<Project>
        {
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project A",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ClientID = 1,
                CampaignType = 1,
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "SEO"
                }
            },
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project B",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                ClientID = 2,
                CampaignType = 2,
                Clients = new Client
                {
                    ClientID = 2,
                    Name = "Client2",
                    Email = "client2@gmail.com",
                    Company = "Company B",
                    Phone = "987654321",
                    Address = "456 Avenue",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 2,
                    Name = "PPC"
                }
            }
        };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                // Act
                List<Project> result = (await service.GetProjects(null, 1, null, null, null)).ToList(); // Filter by CampaignType = 1

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(1, result[0].CampaignType);
                Assert.Equal("Project A", result[0].ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnFilteredProjects_WhenClientFilterIsApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);

                var projects = new List<Project>
        {
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project A",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ClientID = 1,
                CampaignType = 1,
                Clients = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 1,
                    Name = "SEO"
                }
            },
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project B",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                ClientID = 2,
                CampaignType = 2,
                Clients = new Client
                {
                    ClientID = 2,
                    Name = "Client2",
                    Email = "client2@gmail.com",
                    Company = "Company B",
                    Phone = "987654321",
                    Address = "456 Avenue",
                    CreateDate = DateTime.Now
                },
                CampaignTypes = new CampaignType
                {
                    Id = 2,
                    Name = "PPC"
                }
            }
        };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                // Act
                List<Project> result = (await service.GetProjects(null, null, 2, null, null)).ToList(); // Filter by ClientID = 2

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(2, result[0].ClientID);
                Assert.Equal("Project B", result[0].ProjectName);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjects_ShouldReturnFilteredProjects_WhenCampaignAndClientFiltersAreApplied()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDBContext(options))
            {
                var service = new ProjectQuery(context);
                
                var campaignType1 = new CampaignType
                {
                    Id = 1,
                    Name = "SEO"
                };

                var campaignType2 = new CampaignType
                {
                    Id = 2,
                    Name = "PPC"
                };

                var client1 = new Client
                {
                    ClientID = 1,
                    Name = "Client1",
                    Email = "client1@gmail.com",
                    Company = "Company A",
                    Phone = "123456789",
                    Address = "123 Street",
                    CreateDate = DateTime.Now
                };

                var client2 = new Client
                {
                    ClientID = 2,
                    Name = "Client2",
                    Email = "client2@gmail.com",
                    Company = "Company B",
                    Phone = "987654321",
                    Address = "456 Avenue",
                    CreateDate = DateTime.Now
                };

                var projects = new List<Project>
        {
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project A",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                ClientID = 1,
                CampaignType = 1,
                Clients = client1, 
                CampaignTypes = campaignType1 
            },
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project B",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                ClientID = 2,
                CampaignType = 1,
                Clients = client2, 
                CampaignTypes = campaignType1 
            },
            new Project
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = "Project C",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                ClientID = 1,
                CampaignType = 2,
                Clients = client1, 
                CampaignTypes = campaignType2 
            }
        };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                // Act
                List<Project> result = (await service.GetProjects(null, 1, 1, null, null)).ToList(); // Filter by CampaignType = 1 y ClientID = 1

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(1, result[0].CampaignType);
                Assert.Equal(1, result[0].ClientID);
                Assert.Equal("Project A", result[0].ProjectName);
            }
        }
    }
}
