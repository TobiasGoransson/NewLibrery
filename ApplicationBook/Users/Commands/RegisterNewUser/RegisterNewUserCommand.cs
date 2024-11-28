using ApplicationBook.Users.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    public class RegisterNewUserCommand : IRequest<User>
    {
        public RegisterNewUserCommand(UserDto newUser)
        {
            NewUser = newUser;
        }
        public UserDto NewUser { get;}
    }
}
