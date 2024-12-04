using ApplicationBook.Interfaces.RepoInterfaces;
using ApplicationBook.Users.Queries.LogIn.Helpers;
using Domain;
using MediatR;

namespace ApplicationBook.Users.Queries.LogIn
{
    internal class LogInUserQueryHandler : IRequestHandler<LogInUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly Tokenhelper _tokenhelper;

        public LogInUserQueryHandler(IUserRepository userRepository, Tokenhelper tokenhelper)
        {
            _userRepository = userRepository;
            _tokenhelper = tokenhelper;
        }

        public async Task<string> Handle(LogInUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByCredentialsAsync(request.LogInUser.UserName, request.LogInUser.Password, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            string token = _tokenhelper.GenerateJwtToken(user);
            return token;
        }
    }


}
