using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IUserServices;
using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.UserServices
{
    public class UserGetServices : IUserGetServices
    {
        private readonly IUserQuery _query;

        public UserGetServices(IUserQuery query)
        {
            _query = query;
        }

        public async Task<List<Users>> GetAll()
        {
            var result = await _query.GetListUsers();



            List<Users> usersList = new List<Users>();

            foreach (var item in result)
            {

                Users users = new Users();
                users.Name = item.Name;
                users.Id = item.UserID;
                users.Email = item.Email;

                usersList.Add(users);
            }
            return usersList;
        }

    }
}
