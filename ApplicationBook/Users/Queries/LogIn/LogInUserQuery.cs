using ApplicationBook.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Queries.LogIn
{
    public class LogInUserQuery : IRequest<string>
    {
       public LogInUserQuery(UserDto logInUser)
        {
            LogInUser = logInUser;
        }

        public UserDto LogInUser { get; }
    }
}
