using Application.Models;
using Application.Request;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.IServices.IProjectServices
{
    public interface IProjectPostServices
    {
        public Task<ProjectDetails> CreateProject(ProjectRequest request);

    }


}
