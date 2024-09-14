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
    public class CampaignTypeQuery : ICampaignTypeQuery
    {
        private readonly AppDBContext _context;

        public CampaignTypeQuery(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<CampaignType>> GetListCampaignTypes()
        {
            var result = await _context.CampaignTypes.ToListAsync();

            return result;
        }
    }
}
