using Domain.Dtos; // Importera OperationResult
using Domain;
using MediatR;

namespace ApplicationBook.Users.Queries.LogIn
{
    public class LogInUserQuery : IRequest<OperationResult<string>>
    {
        public LogInUserQuery(UserDto logInUser)
        {
            LogInUser = logInUser;
        }

        public UserDto LogInUser { get; }
    }
}
