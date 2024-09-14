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

        public async Task<Tasks> UpdateTask(Guid taskID, TasksRequest tRequest)
        {
             await _validator.Validate(tRequest);

            try
            {
                var task = await _taskQuery.GetTaskById(taskID);

                if (task == null)
                {
                    throw new NotFoundException("Task not found");
                }

                task.Name = tRequest.Name;
                task.DueDate = tRequest.DueDate;
                task.Status = tRequest.Status;
                task.AssignedTo = tRequest.User;
                task.UpdateDate = DateTime.Now;

                await _projectCommand.UpdateProjectTasks(task);

                var uTask = await _taskQuery.GetTaskById(taskID);

                if (uTask == null)
                {
                    throw new NotFoundException("Task not found");
                }

                var tasks = new Tasks
                {
                    Id = uTask.TaskID,
                    Name = uTask.Name,
                    DueDate = uTask.DueDate,
                    ProjectId = uTask.ProjectID,
                    Status = new GenericResponse
                    {
                        Id = uTask.TaskStatus.Id,
                        Name = uTask.TaskStatus.Name
                    },
                    UserAssigned = new Users
                    {
                        Id = uTask.Users.UserID,
                        Name = uTask.Users.Name,
                        Email = uTask.Users.Email
                    }
                };
                return tasks;
            }
            catch (Exception ex) {
                throw new NotFoundException(ex.Message);
            }
        }

    }
}



