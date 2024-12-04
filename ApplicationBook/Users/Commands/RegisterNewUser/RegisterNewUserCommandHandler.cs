using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    internal sealed class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, User>
    {
        private readonly FakeDatabase _fakeDatabase;

        public RegisterNewUserCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<User> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            User newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };
            _fakeDatabase.Users.Add(newUser);
            return Task.FromResult(newUser);
        }
    }
}
