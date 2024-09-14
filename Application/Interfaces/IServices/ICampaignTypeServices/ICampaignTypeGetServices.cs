using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.ICampaignTypeServices
{
    public interface ICampaignTypeGetServices
    {
        public Task<List<GenericResponse>> GetAll();
    }
}
