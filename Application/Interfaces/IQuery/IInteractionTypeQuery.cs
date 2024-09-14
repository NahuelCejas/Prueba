using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IQuery
{
    public interface IInteractionTypeQuery
    {
        public Task<List<InteractionType>> GetListInteractionTypes();

        public Task<InteractionType> GetInteractionTypeById(int id);
    }
}
