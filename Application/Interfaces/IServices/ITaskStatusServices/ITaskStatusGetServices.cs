using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.ITaskStatusServices
{
    public interface ITaskStatusGetServices
    {
        public Task<List<GenericResponse>> GetAll();
    }
}
