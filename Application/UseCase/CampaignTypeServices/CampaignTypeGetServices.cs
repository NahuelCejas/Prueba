using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.ICampaignTypeServices;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.CampaignTypesServices
{
    public class CampaignTypeGetServices : ICampaignTypeGetServices
    {
        private readonly ICampaignTypeQuery _query;

        public CampaignTypeGetServices(ICampaignTypeQuery query)
        {
            _query = query;
        }

        public async Task<List<GenericResponse>> GetAll()
        {
            var result = await _query.GetListCampaignTypes();

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
