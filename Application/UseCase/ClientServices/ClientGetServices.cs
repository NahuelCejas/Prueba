using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IClientServices;
using Application.Models;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.ClientServices
{
    public class ClientGetServices : IClientGetServices
    {
        private readonly IClientQuery _query;

        public ClientGetServices(IClientQuery query)
        {
            _query = query;
        }

        public async Task<List<Clients>> GetAll()
        {
            var list = await _query.GetListClients();
            List<Clients> listClients = new List<Clients>();

            foreach (Client item in list)
            {
                listClients.Add(MapClientToResponse(item));
            }

            return listClients;            
        }

        private static Clients MapClientToResponse(Client item)
        {
            return new Clients
            {
                Id = item.ClientID,
                Name = item.Name,
                Phone = item.Phone,
                Address = item.Address,
                Company = item.Company,
                Email = item.Email
            };
        }
    }
}
