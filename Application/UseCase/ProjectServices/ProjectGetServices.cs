using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IProjectServices;
using Application.Models;
using Application.Request;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.ProjectServices
{
    public class ProjectGetServices : IProjectGetServices
    {

        private readonly IProjectQuery _projectQuery;
        public ProjectGetServices(IProjectQuery projectQuery)
        {
            _projectQuery = projectQuery;
        }

        public async Task<ProjectDetails> GetProjectById(Guid projectId)
        {
            try
            {
                Domain.Entities.Project project = await _projectQuery.GetProjectById(projectId);

                if (project == null)
                {
                    throw new NotFoundException("Project not found");
                }


                var data = new Application.Response.Project
                {
                    Id = project.ProjectID,
                    Name = project.ProjectName,
                    Start = project.StartDate,
                    End = project.EndDate,
                    Client = new Clients
                    {
                        Id = project.ClientID,
                        Name = project.Clients.Name,
                        Email = project.Clients.Email,
                        Company = project.Clients.Company,
                        Address = project.Clients.Address,
                        Phone = project.Clients.Phone,
                    },
                    CampaignType = new GenericResponse
                    {
                       Id = project.CampaignTypes.Id,
                       Name = project.CampaignTypes.Name,
                    }
                };


                var interactions = project.ListInteractions.Select(interaction => new Interactions
                {
                    Id = interaction.InteractionID,
                    Notes = interaction.Notes,
                    Date = interaction.Date,
                    ProjectId = interaction.ProjectID,
                    InteractionType = new GenericResponse
                    {
                        Id = interaction.InteractionTypes.Id,
                        Name = interaction.InteractionTypes.Name
                    }
                }).ToList();


                var tasks = project.ListTasks.Select(task => new Tasks
                {
                    Id = task.TaskID,
                    Name = task.Name,
                    DueDate = task.DueDate,
                    ProjectId = task.ProjectID,
                    Status = new GenericResponse
                    {
                        Id = task.TaskStatus.Id,
                        Name = task.TaskStatus.Name
                    },
                    UserAssigned = new Users
                    {
                        Id = task.Users.UserID,
                        Name = task.Users.Name,
                        Email = task.Users.Email
                    }
                }).ToList();


                return await System.Threading.Tasks.Task.FromResult(new ProjectDetails
                {
                    Data = data,
                    Interactions = interactions,
                    Tasks = tasks
                });
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task<List<Response.Project>> GetProjects(string? name, int? campaign, int? client, int? offset, int? size)
        {
            var projects = await _projectQuery.GetProjects(name, campaign, client, offset, size);

            var responseProjects = new List<Response.Project>();

            foreach (var project in projects)
            {
                var responseProject = new Response.Project
                {
                    Id = project.ProjectID,
                    Name = project.ProjectName,
                    Start = project.StartDate,
                    End = project.EndDate,
                    Client = new Clients
                    {
                        Id = project.Clients.ClientID,
                        Name = project.Clients.Name,
                        Email = project.Clients.Email,
                        Company = project.Clients.Company,
                        Phone = project.Clients.Phone,
                        Address = project.Clients.Address
                    },
                    CampaignType = new GenericResponse
                    {
                        Id = project.CampaignTypes.Id,
                        Name = project.CampaignTypes.Name
                    }
                };

                responseProjects.Add(responseProject);
            }

            return responseProjects;
        }
    }


}
