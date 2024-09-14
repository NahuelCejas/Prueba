using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;


namespace Application.Interfaces.ICommand
{
    public interface IClientCommand
    {
        public Task InsertClient(Client client);
    }
}
