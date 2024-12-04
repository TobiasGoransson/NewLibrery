using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.Id);
            if (author != null)
            {
                author.FirstName = request.FirstName;
                author.LastName = request.LastName;
            }
            return Task.FromResult(author);
        }
    }
}
