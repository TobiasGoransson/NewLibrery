using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    internal sealed class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, OperationResult<User>>
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<RegisterNewUserCommandHandler> _logger;

        public RegisterNewUserCommandHandler(IRepository<User> repository, ILogger<RegisterNewUserCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterNewUserCommand for creating a new user: {UserName}", request.NewUser.UserName);

            // Kontrollera att användardata är giltig
            if (string.IsNullOrWhiteSpace(request.NewUser.UserName) || string.IsNullOrWhiteSpace(request.NewUser.Password))
            {
                _logger.LogWarning("Invalid data provided for registering user.");
                return OperationResult<User>.Failure("Username and password must be provided.");
            }

            // Skapa en ny användare
            User newUser = new User
            {
                UId = 0,
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };

            try
            {
                // Lägg till användaren i repositoryn
                await _repository.CreateAsync(newUser);
                _logger.LogInformation("Successfully registered new user with UserName: {UserName}", newUser.UserName);

                // Returnera ett framgångsrikt resultat
                return OperationResult<User>.Successfull(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user: {UserName}", request.NewUser.UserName);

                // Returnera ett felresultat
                return OperationResult<User>.Failure("An error occurred while registering the user. Please try again later.");
            }
        }
    }
}
