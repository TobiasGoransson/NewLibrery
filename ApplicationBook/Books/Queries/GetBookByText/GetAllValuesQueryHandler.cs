using Domain;
using Infrastructur.Database;
using MediatR;

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
