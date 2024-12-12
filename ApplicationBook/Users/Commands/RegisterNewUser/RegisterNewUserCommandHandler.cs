using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    internal sealed class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, User>
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<RegisterNewUserCommandHandler> _logger; // Lägg till logger

        public RegisterNewUserCommandHandler(IRepository<User> repository, ILogger<RegisterNewUserCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<User> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterNewUserCommand for creating a new user: {UserName}", request.NewUser.UserName); // Logga när kommandot hanteras

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
                _logger.LogInformation("Successfully registered new user with UserName: {UserName}", newUser.UserName); // Logga framgång

                // Returnera den nyregistrerade användaren
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user: {UserName}", request.NewUser.UserName); // Logga fel vid registrering
                throw; // Kasta vidare undantaget
            }
        }
    }
}
