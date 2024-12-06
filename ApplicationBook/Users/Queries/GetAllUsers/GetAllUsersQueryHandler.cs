using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger; // Lägg till logger

        public GetAllUsersQueryHandler(IRepository<User> repository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery to fetch all users."); // Logga när förfrågan hanteras

            // Hämta alla användare från repositoryt
            var allUsers = await _repository.GetAllAsync();

            _logger.LogInformation("Successfully fetched {UserCount} users from the repository.", allUsers.Count); // Logga när användarna har hämtats

            return allUsers;
        }
    }
}
