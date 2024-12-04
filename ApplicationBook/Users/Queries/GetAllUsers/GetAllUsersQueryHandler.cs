using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAllUsersQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<User> AllUsersFromFakedatabase = _fakeDatabase.Users;
            return Task.FromResult(AllUsersFromFakedatabase);
        }

    }
}
