using Domain;
using Infrastructur.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAuthorByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            
            if (request.Id <= 0)
            {
                throw new ArgumentException("The ID must be a positive integer.");
            }

            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.Id);

            
            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} was not found.");
            }

            return Task.FromResult(author);
        }
    }

}
