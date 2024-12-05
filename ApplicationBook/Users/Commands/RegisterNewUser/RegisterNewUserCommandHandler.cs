using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Users.Commands.RegisterNewUser
{
    internal sealed class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, User>
    {
        private readonly IRepository<User> _repository;

        public RegisterNewUserCommandHandler(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
        {
            // Skapa en ny användare
            User newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };

            // Lägg till användaren i repositoryn
            await _repository.CreateAsync(newUser);

            // Returnera den nyregistrerade användaren
            return newUser;
        }
    }

}
