using Application.Exceptions;
using Application.Interfaces.IQuery;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class ProjectQuery : IProjectQuery
    {
        private readonly AppDBContext _context;

        public ProjectQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Project> GetProjectById(Guid id)
        {
            var project = await _context.Set<Project>()
                .Include(p => p.Clients)
                .Include (p => p.CampaignTypes)
                .Include(p => p.ListInteractions)
                    .ThenInclude(i => i.InteractionTypes)
                .Include(p => p.ListTasks)
                    .ThenInclude(t => t.TaskStatus)
                .Include(p => p.ListTasks)
                    .ThenInclude(t => t.Users)
                .FirstOrDefaultAsync(p => p.ProjectID == id);

            if (project == null)
            {
                throw new NotFoundException("Project not found");
            }

            return project;
        }

        public async Task<Project>  GetProjectByName(string name)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectName == name);

            if (project == null)
            {
                throw new NotFoundException("Project not found");
            }

            return project;
        }

        public async Task<IEnumerable<Project>> GetProjects(string? name, int? campaign, int? client, int? offset, int? size)
        {
            var projectQuery = _context.Projects
                .Include(p => p.Clients)
                .Include(p => p.CampaignTypes)
                .Include(p => p.ListInteractions)
                    .ThenInclude(i => i.InteractionTypes)
                .Include(p => p.ListTasks)
                    .ThenInclude(j => j.TaskStatus)
                .Include(p => p.ListTasks)
                    .ThenInclude(j => j.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                projectQuery = projectQuery.Where(p => p.ProjectName.Contains(name));
            }

            if (client.HasValue)
            {
                projectQuery = projectQuery.Where(pc => pc.ClientID == client.Value);
            }

            if (campaign.HasValue)
            {
                projectQuery = projectQuery.Where(pc => pc.CampaignType == campaign.Value);
            }

            if (offset.HasValue)
            {
                projectQuery = projectQuery.Skip(offset.Value);
            }

            if (size.HasValue)
            {
                projectQuery = projectQuery.Take(size.Value);
            }

            return await projectQuery.ToListAsync();
        }
    }

    
}
