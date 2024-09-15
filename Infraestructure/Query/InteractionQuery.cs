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
    public class InteractionQuery : IInteractionQuery
    {
        private readonly AppDBContext _context;
        public InteractionQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Interaction> GetInteractionById(Guid id)
        {
            var interaction = await _context.Interactions
                .Include(i => i.InteractionTypes)
                .FirstOrDefaultAsync(c => c.InteractionID == id);

            if (interaction == null)
            {
                throw new NotFoundException("Interaction not found"); 
            }

            return interaction;
        }
    }
}
