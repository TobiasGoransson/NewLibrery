using ApplicationBook.Interfaces.RepoInterfaces;
using ApplicationBook.Users.Queries.LogIn.Helpers;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Queries.LogIn
{
    internal class LogInUserQueryHandler : IRequestHandler<LogInUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly Tokenhelper _tokenhelper;
        private readonly ILogger<LogInUserQueryHandler> _logger; // Lägg till logger

        public LogInUserQueryHandler(IUserRepository userRepository, Tokenhelper tokenhelper, ILogger<LogInUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _tokenhelper = tokenhelper;
            _logger = logger; // Spara loggern
        }

        public async Task<string> Handle(LogInUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LogInUserQuery for user: {UserName}.", request.LogInUser.UserName); // Logga när inloggning påbörjas

            var user = await _userRepository.GetByCredentialsAsync(request.LogInUser.UserName, request.LogInUser.Password, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {UserName}. Invalid username or password.", request.LogInUser.UserName); // Logga när inloggning misslyckas
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generera JWT token
            string token = _tokenhelper.GenerateJwtToken(user);

            _logger.LogInformation("User {UserName} successfully logged in. JWT token generated.", request.LogInUser.UserName); // Logga när inloggning är framgångsrik

            return token;
        }
    }
}
