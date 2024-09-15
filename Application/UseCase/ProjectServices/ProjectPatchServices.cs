using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IProjectServices;
using Application.Interfaces.IValidator;
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
    public class ProjectPatchServices : IProjectPatchServices
    {

        private readonly IProjectQuery _projectQuery;
        private readonly IProjectCommand _projectCommand;
        private readonly ITaskQuery _taskQuery;
        private readonly IInteractionQuery _interactionQuery;
        private readonly IValidatorHandler<TasksRequest> _tasksValidator;
        private readonly IValidatorHandler<InteractionsRequest> _interactionsValidator;

        public ProjectPatchServices(IProjectQuery projectQuery, IProjectCommand projectCommand, ITaskQuery taskQuery, IInteractionQuery interactionQuery, IValidatorHandler<TasksRequest> tasksValidator, IValidatorHandler<InteractionsRequest> interactionsValidator)
        {
            _projectQuery = projectQuery;
            _projectCommand = projectCommand;
            _taskQuery = taskQuery;
            _interactionQuery = interactionQuery;
            _tasksValidator = tasksValidator;
            _interactionsValidator = interactionsValidator;
        }


        public async Task<Interactions> AddInteraction(Guid projectId, InteractionsRequest request)
        {
             await _interactionsValidator.Validate(request);
            try
            {
                Domain.Entities.Project project = await _projectQuery.GetProjectById(projectId);

                if (project == null)
                {
                    throw new NotFoundException("Project not found");
                }

                var nInteraction = new Interaction
                {
                    Notes = request.Notes,
                    Date = request.Date,
                    InteractionType = request.InteractionType,
                    ProjectID = projectId

                };

                await _projectCommand.AddProjectInteractions(nInteraction);

                project.UpdateDate = DateTime.Now;
                await _projectCommand.UpdateProject(project);

                var interactionAdd = await _interactionQuery.GetInteractionById(nInteraction.InteractionID);

                if (interactionAdd == null)
                {
                    throw new NotFoundException("Interaction not found");
                }

                return new Interactions
                {
                    Id = interactionAdd.InteractionID,
                    ProjectId = interactionAdd.ProjectID,
                    Notes = interactionAdd.Notes,
                    Date = interactionAdd.Date,
                    
                    InteractionType = new GenericResponse
                    {
                        Id = interactionAdd.InteractionTypes.Id,
                        Name = interactionAdd.InteractionTypes.Name
                    }
                };
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task<Tasks> AddTask(Guid projectId, TasksRequest request)
        {
            await _tasksValidator.Validate(request);

            try
            {
                Domain.Entities.Project project = await _projectQuery.GetProjectById(projectId);

                if (project == null)
                {
                    throw new NotFoundException("Project not found");
                }

                var nTask = new Domain.Entities.Task
                {
                    Name = request.Name,
                    ProjectID = projectId,
                    Status = request.Status,
                    AssignedTo = request.User,
                    DueDate = request.DueDate,
                    CreateDate = DateTime.Now,
                };

                await _projectCommand.AddProjectTasks(nTask);

                project.UpdateDate = DateTime.Now;

                await _projectCommand.UpdateProject(project);

                var taskAdd = await _taskQuery.GetTaskById(nTask.TaskID);

                return new Tasks
                {
                    Id = taskAdd.TaskID,
                    Name = taskAdd.Name,
                    DueDate = taskAdd.DueDate,
                    ProjectId = taskAdd.ProjectID,
                    Status = new GenericResponse
                    {
                        Id = taskAdd.TaskStatus.Id,
                        Name = taskAdd.TaskStatus.Name
                    },
                    UserAssigned = new Users
                    {
                        Id = taskAdd.Users.UserID,
                        Name = taskAdd.Users.Name,
                        Email = taskAdd.Users.Email
                    }
                };

            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }
    }


}
