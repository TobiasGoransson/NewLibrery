using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructur.Database;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetAllValuesQueryHandler : IRequestHandler<GetAllValuesQuery, List<Book>>
    {
        private readonly FakeDatabase _database;

        public GetAllValuesQueryHandler(FakeDatabase database)
        {
            _database = database;
        }

        public async Task<List<Book>> Handle(GetAllValuesQuery request, CancellationToken cancellationToken)
        {
            
            var books = _database.Books;

            // Simulera asynkron hantering
            return await Task.FromResult(books);
        }
    }
}
