using Domain;
using Infrastructur.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public DeleteAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.Id);
            if (author != null)
            {
                _fakeDatabase.Authors.Remove(author); // Ta bort författaren
            }
            return Task.FromResult(author); // Returnera den borttagna författaren (eller null om inte funnen)
        }
    }
}
