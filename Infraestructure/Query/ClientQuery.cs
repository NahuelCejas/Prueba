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
    public class ClientQuery : IClientQuery
    {
        private readonly AppDBContext _context;

        public ClientQuery(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<Client>> GetListClients()
        {
            var result = _context.Clients.ToListAsync();
            return result;
        }


        public async Task<bool> ClientExists(int id)
        {
            var result = await _context.Clients.AnyAsync(c => c.ClientID == id);
            return result;
        }
    }
}
