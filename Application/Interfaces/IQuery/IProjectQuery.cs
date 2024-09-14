using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IQuery
{
    public interface IProjectQuery
    {
        public Task<Project> GetProjectById(Guid id);
        public Task<Project> GetProjectByName(string name);
        public Task<IEnumerable<Project>> GetProjects(string? name, int? campaign, int? client, int? offset, int? size);
        
    }
}
