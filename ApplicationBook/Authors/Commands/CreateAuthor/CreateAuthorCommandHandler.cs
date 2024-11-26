using ApplicationBook.Authors.Queries.GetAllAuthors;
using Domain;
using Infrastructur.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Commands.CreateAuthor
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<Author>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAllAuthorsQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<List<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            // Kontrollera att databasen inte är null
            if (_fakeDatabase.Authors == null)
            {
                return Task.FromResult(new List<Author>()); // Returnerar en tom lista istället för null
            }

            return Task.FromResult(_fakeDatabase.Authors);
        }
    }
}
