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

                Interaction nInteraction = MapRequestToInteraction(projectId, request);
                await _projectCommand.AddProjectInteractions(nInteraction);               

                project.UpdateDate = DateTime.Now;
                await _projectCommand.UpdateProject(project);

                var interactionAdd = await _interactionQuery.GetInteractionById(nInteraction.InteractionID);

                if (interactionAdd == null)
                {
                    throw new NotFoundException("Interaction not found");
                }

                return MapInteractionToResponse(interactionAdd);                
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

                var nTask = MapRequestToTask(projectId, request);
                await _projectCommand.AddProjectTasks(nTask);

                project.UpdateDate = DateTime.Now;
                await _projectCommand.UpdateProject(project);

                var taskAdd = await _taskQuery.GetTaskById(nTask.TaskID);
                if (taskAdd == null)
                {
                    throw new NotFoundException("Task not found");
                }

                return MapTaskToResponse(taskAdd);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }

        private static Interaction MapRequestToInteraction(Guid projectId, InteractionsRequest request)
        {
            return new Interaction
            {
                Notes = request.Notes!,
                Date = request.Date,
                InteractionType = request.InteractionType,
                ProjectID = projectId
            };
        }

        private static Interactions MapInteractionToResponse(Interaction interaction)
        {
            return new Interactions
            {
                Id = interaction.InteractionID,
                ProjectId = interaction.ProjectID,
                Notes = interaction.Notes,
                Date = interaction.Date,
                InteractionType = interaction.InteractionTypes != null ? new GenericResponse
                {
                    Id = interaction.InteractionTypes.Id,
                    Name = interaction.InteractionTypes.Name
                } : null
            };
        }

        private static Domain.Entities.Task MapRequestToTask(Guid projectId, TasksRequest request)
        {
            return new Domain.Entities.Task
            {
                Name = request.Name!,
                ProjectID = projectId,
                Status = request.Status,
                AssignedTo = request.User,
                DueDate = request.DueDate,
                CreateDate = DateTime.Now,
            };
        }

        private static Tasks MapTaskToResponse(Domain.Entities.Task task)
        {
            return new Tasks
            {
                Id = task.TaskID,
                Name = task.Name,
                DueDate = task.DueDate,
                ProjectId = task.ProjectID,
                Status = task.TaskStatus != null ? new GenericResponse
                {
                    Id = task.TaskStatus.Id,
                    Name = task.TaskStatus.Name
                } : null,
                UserAssigned = task.Users != null ? new Users
                {
                    Id = task.Users.UserID,
                    Name = task.Users.Name,
                    Email = task.Users.Email
                } : null
            };
        }
    }

}
