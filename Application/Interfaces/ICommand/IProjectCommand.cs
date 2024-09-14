using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.ICommand
{
    public interface IProjectCommand
    {
        public Task InsertProject(Project project);
        public Task AddProjectInteractions(Interaction interaction);
        public Task UpdateProject(Project project);
        public Task AddProjectTasks(Domain.Entities.Task task);
        public Task UpdateProjectTasks(Domain.Entities.Task task);
    }
}
