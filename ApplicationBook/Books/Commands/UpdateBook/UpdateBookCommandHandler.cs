using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly IRepository<Book> _repository;

        public UpdateBookCommandHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // Hitta boken med det givna ID:t
            var existingBook = await _repository.GetByIdAsync(request.UpdatedBook.Id, cancellationToken);

            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Ingen bok hittades med ID {request.UpdatedBook.Id}.");
            }

            // Uppdatera bokens egenskaper
            existingBook.Title = request.UpdatedBook.Title;
            existingBook.Description = request.UpdatedBook.Description;
            existingBook.Author = request.UpdatedBook.Author;

            // Uppdatera boken i databasen
            await _repository.UpdateAsync(existingBook, cancellationToken);

            // Returnera den uppdaterade boken
            return existingBook;
        }
    }


}
