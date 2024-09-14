using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IClientServices;
using Application.Interfaces.IValidator;
using Application.Models;
using Application.Request;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.ClientServices
{
    public class ClientPostServices : IClientPostServices
    {
        private readonly IClientCommand _command;
        private readonly IValidatorHandler<ClientsRequest> _validator;

        public ClientPostServices(IClientCommand command, IValidatorHandler<ClientsRequest> validator)
        {
            _command = command;
            _validator = validator;
        }


        public async Task<Clients> CreateClient(ClientsRequest request)
        {
            await _validator.Validate(request);

            Client client = new Client();
            {
                client.Name = request.Name;
                client.Address = request.Address;
                client.Phone = request.Phone;
                client.Company = request.Company;
                client.Email = request.Email;
                client.CreateDate = DateTime.Now;
            }
            await _command.InsertClient(client);

            Clients clientResponse = new Clients();
            {
                clientResponse.Id = client.ClientID;
                clientResponse.Name = client.Name;
                clientResponse.Address = client.Address;
                clientResponse.Phone = client.Phone;
                clientResponse.Company = client.Company;
                clientResponse.Email = client.Email;
            }

            return clientResponse;
        }

       
    }
}
