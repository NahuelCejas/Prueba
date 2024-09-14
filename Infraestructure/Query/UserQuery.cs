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
    public class UserQuery : IUserQuery
    {
        private readonly AppDBContext _context;

        public UserQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetListUsers()
        {
            var result = await _context.Users.ToListAsync();
            return result;
        }
       
    }
}
