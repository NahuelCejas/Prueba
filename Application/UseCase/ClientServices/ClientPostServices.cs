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

            Client client = MapRequestToClient(request);
            await _command.InsertClient(client);

            Clients clientResponse = MapClientToResponse(client);
            return clientResponse;
        }

        private static Client MapRequestToClient(ClientsRequest request)
        {
            return new Client
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Company = request.Company,
                Email = request.Email,
                CreateDate = DateTime.Now
            };
        }

        private static Clients MapClientToResponse(Client client)
        {
            return new Clients
            {
                Id = client.ClientID,
                Name = client.Name,
                Address = client.Address,
                Phone = client.Phone,
                Company = client.Company,
                Email = client.Email
            };
        }
    }
}
