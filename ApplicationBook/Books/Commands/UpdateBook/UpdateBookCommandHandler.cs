using Domain;
using Infrastructur.Database;
using MediatR;

namespace ApplicationBook.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly FakeDatabase fakeDatabase;

        public UpdateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // Hitta boken med det givna ID:t
            var existingBook = fakeDatabase.Books.FirstOrDefault(b => b.Id == request.UpdatedBook.Id);

            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Ingen bok hittades med ID {request.UpdatedBook.Id}.");
            }

            // Uppdatera bokens egenskaper
            existingBook.Title = request.UpdatedBook.Title;
            existingBook.Description = request.UpdatedBook.Description;
            existingBook.Author = request.UpdatedBook.Author;

            // Returnera den uppdaterade boken
            return Task.FromResult(existingBook);
        }
    }

}
