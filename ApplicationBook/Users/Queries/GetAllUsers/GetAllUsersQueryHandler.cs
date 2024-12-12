using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IRepository<User> repository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery to fetch all users.");

            try
            {
                // Hämta alla användare från repositoryt
                var allUsers = await _repository.GetAllAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    _logger.LogWarning("No users found in the repository.");
                    return OperationResult<List<User>>.Failure("No users were found.");
                }

                _logger.LogInformation("Successfully fetched {UserCount} users from the repository.", allUsers.Count);

                // Returnera ett framgångsrikt resultat
                return OperationResult<List<User>>.Successfull(allUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");

                // Returnera ett felresultat
                return OperationResult<List<User>>.Failure("An error occurred while fetching users. Please try again later.");
            }
        }
    }
}
