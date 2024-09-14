using Application.Interfaces.IQuery;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class TaskStatusQuery : ITaskStatusQuery
    {
        private readonly AppDBContext _context;

        public TaskStatusQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.TaskStatus>> GetListTaskStatus()
        {
           var list = await _context.Taskstatus.ToListAsync();

            return list;
        }
    }
}
