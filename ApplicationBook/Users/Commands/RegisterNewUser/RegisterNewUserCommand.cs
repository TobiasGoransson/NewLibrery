using ApplicationBook.Users.Dtos;
using Domain;
using MediatR;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    public class RegisterNewUserCommand : IRequest<User>
    {
        public RegisterNewUserCommand(UserDto newUser)
        {
            NewUser = newUser;
        }
        public UserDto NewUser { get; }
    }
}
