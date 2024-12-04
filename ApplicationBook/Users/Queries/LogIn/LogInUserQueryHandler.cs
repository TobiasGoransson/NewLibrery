using ApplicationBook.Users.Queries.LogIn.Helpers;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Users.Queries.LogIn
{
    internal class LogInUserQueryHandler : IRequestHandler<LogInUserQuery, string>
    {
        private readonly FakeDatabase _fakeDatabase;
        private readonly Tokenhelper _tokenhelper;
        public LogInUserQueryHandler(FakeDatabase fakeDatabase, Tokenhelper tokenhelper)
        {
            _fakeDatabase = fakeDatabase;
            _tokenhelper = tokenhelper;
        }

        public Task<string> Handle(LogInUserQuery request, CancellationToken cancellationToken)
        {
            var user = _fakeDatabase.Users.FirstOrDefault(u => u.UserName == request.LogInUser.UserName && u.Password == request.LogInUser.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
            string token = _tokenhelper.GenerateJwtToken(user);
            return Task.FromResult(token);
        }
    }
}
