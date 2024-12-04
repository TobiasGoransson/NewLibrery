using ApplicationBook.Users.Dtos;
using MediatR;

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
