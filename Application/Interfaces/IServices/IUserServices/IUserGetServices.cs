using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IUserServices
{
    public interface IUserGetServices
    {
        public Task<List<Users>> GetAll();
    }
}
