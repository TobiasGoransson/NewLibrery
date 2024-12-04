using Domain;
using MediatR;

namespace ApplicationBook.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<User>>
    {
    }
}
