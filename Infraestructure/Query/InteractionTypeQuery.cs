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
    public class InteractionTypeQuery : IInteractionTypeQuery
    {
        private readonly AppDBContext _context;

        public InteractionTypeQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<InteractionType>> GetListInteractionTypes()
        {
            var result= await _context.InteractionTypes.ToListAsync();
            return result;
        }

        public async Task<InteractionType> GetInteractionTypeById(int id)
        {
            var result = await _context.InteractionTypes.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }
    }
}
