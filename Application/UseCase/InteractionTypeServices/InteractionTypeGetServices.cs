using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IInteractionTypeServices;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.InteractionTypeServices
{
    public class InteractionTypeGetServices : IInteractionTypeGetServices
    {
        private IInteractionTypeQuery _query;

        public InteractionTypeGetServices(IInteractionTypeQuery query)
        {
            _query = query;
        }

        public async Task<List<GenericResponse>> GetAll()
        {
            var result = await _query.GetListInteractionTypes();

            List<GenericResponse> list = new List<GenericResponse>();

            foreach (var item in result)
            {
                GenericResponse response = new GenericResponse();
                response.Id = item.Id;
                response.Name = item.Name;


                list.Add(response);
            }

            return list;
        }
    }
}
