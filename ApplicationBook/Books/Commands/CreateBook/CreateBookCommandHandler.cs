using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly FakeDatabase fakeDatabase;

        public CreateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            // Beräkna nästa ID baserat på böcker som redan finns i databasen
            int nextId = fakeDatabase.Books.Any() ? fakeDatabase.Books.Max(b => b.Id) + 1 : 1;
            request.NewBook.Id = nextId;

            // Lägg till den nya boken i databasen
            fakeDatabase.Books.Add(request.NewBook);

            // Returnera den uppdaterade listan med böcker
            return Task.FromResult(fakeDatabase.Books);
        }
    }
}
