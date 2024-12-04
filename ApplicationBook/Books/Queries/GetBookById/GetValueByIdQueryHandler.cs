using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBookById
{
    public class GetValueByIdQueryHandler : IRequestHandler<GetValueByIdQuery, Book>
    {
        private readonly FakeDatabase _database;

        public GetValueByIdQueryHandler(FakeDatabase database)
        {
            _database = database;
        }

        public async Task<Book> Handle(GetValueByIdQuery request, CancellationToken cancellationToken)
        {
            // Kontrollera om databasen är korrekt initialiserad
            if (_database == null || _database.Books == null)
            {
                throw new InvalidOperationException("Database is not initialized.");
            }

            // Hämta boken baserat på ID
            var book = _database.Books.FirstOrDefault(b => b.Id == request.Id);

            return await Task.FromResult(book);
        }
    }

}
