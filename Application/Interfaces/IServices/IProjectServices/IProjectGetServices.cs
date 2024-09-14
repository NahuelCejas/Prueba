using Application.Models;
using Application.Request;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IProjectServices
{
    public interface IProjectGetServices
    {
        public Task<ProjectDetails> GetProjectById(Guid projectId);

        Task<List<Response.Project>> GetProjects(string? name, int? campaign, int? client, int? offset, int? size);

    }


}
