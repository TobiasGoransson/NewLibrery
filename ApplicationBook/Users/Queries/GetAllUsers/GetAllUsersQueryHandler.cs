using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly IRepository<User> _repository;

        public GetAllUsersQueryHandler(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // Hämta alla användare från repositoryn
            var allUsers = await _repository.GetAllAsync();

            return allUsers;
        }
    }

}
