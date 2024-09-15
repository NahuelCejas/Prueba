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
    public class ProjectPutServices : IProjectPutServices
    {
        private readonly IProjectCommand _projectCommand;
        private readonly ITaskQuery _taskQuery;
        private readonly IValidatorHandler<TasksRequest> _validator;

        public ProjectPutServices(IProjectCommand projectCommand, ITaskQuery taskQuery, IValidatorHandler<TasksRequest> validator)
        {
            _projectCommand = projectCommand;
            _taskQuery = taskQuery;
            _validator = validator;
        }

        public async Task<Tasks> UpdateTask(Guid taskId, TasksRequest request)
        {
            await _validator.Validate(request);

            try
            {
                var task = await _taskQuery.GetTaskById(taskId);
                if (task == null)
                {
                    throw new NotFoundException("Task not found");
                }

                UpdateTaskFromRequest(task, request);
                await _projectCommand.UpdateProjectTasks(task);

                var uTask = await _taskQuery.GetTaskById(taskId);
                if (uTask == null)
                {
                    throw new NotFoundException("Task not found");
                }

                return MapTaskToResponse(uTask);
            }
            catch (Exception ex) 
            {
                throw new NotFoundException(ex.Message);
            }
        }

        private static void UpdateTaskFromRequest(Domain.Entities.Task task, TasksRequest request)
        {
            task.Name = request.Name!;
            task.DueDate = request.DueDate;
            task.Status = request.Status;
            task.AssignedTo = request.User;
            task.UpdateDate = DateTime.Now;
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



