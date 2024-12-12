using ApplicationBook.Interfaces.RepoInterfaces;
using ApplicationBook.Users.Queries.LogIn.Helpers;
using ApplicationBook.OperationResults; // Importera OperationResult
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Queries.LogIn
{
    internal class LogInUserQueryHandler : IRequestHandler<LogInUserQuery, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Tokenhelper _tokenhelper;
        private readonly ILogger<LogInUserQueryHandler> _logger;

        public LogInUserQueryHandler(IUserRepository userRepository, Tokenhelper tokenhelper, ILogger<LogInUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _tokenhelper = tokenhelper;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(LogInUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LogInUserQuery for user: {UserName}.", request.LogInUser.UserName);

            try
            {
                // Hämta användare baserat på användarnamn och lösenord
                var user = await _userRepository.GetByCredentialsAsync(request.LogInUser.UserName, request.LogInUser.Password, cancellationToken);

                if (user == null)
                {
                    _logger.LogWarning("Login failed for user: {UserName}. Invalid username or password.", request.LogInUser.UserName);
                    return OperationResult<string>.Failure("Invalid username or password.");
                }

                // Generera JWT token
                string token = _tokenhelper.GenerateJwtToken(user);

                _logger.LogInformation("User {UserName} successfully logged in. JWT token generated.", request.LogInUser.UserName);

                return OperationResult<string>.Successfull(token); // Returnera framgång med token
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user: {UserName}.", request.LogInUser.UserName);
                return OperationResult<string>.Failure("An error occurred while processing your login request. Please try again later.");
            }
        }
    }
}
