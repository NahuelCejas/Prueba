using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.ITaskStatusServices;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.TaskStatusServices
{
    public class TaskStatusGetServices : ITaskStatusGetServices
    {
        private readonly ITaskStatusQuery _query;

        public TaskStatusGetServices(ITaskStatusQuery query)
        {
            _query = query;
        }

        public async Task<List<GenericResponse>> GetAll()
        {
            var list = await _query.GetListTaskStatus();



            List<GenericResponse> listResponse = new List<GenericResponse>();

            foreach (var item in list)
            {

                GenericResponse response = new GenericResponse();
                response.Id = item.Id;
                response.Name = item.Name;

                listResponse.Add(response);

            }

            return listResponse;

        }
    }
}
