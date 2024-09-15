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
    public interface IProjectPatchServices
    {
        public Task<Interactions> AddInteraction(Guid projectId, InteractionsRequest request);

        public Task<Tasks> AddTask(Guid projectId, TasksRequest request);

    }


}
