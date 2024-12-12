using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<UpdateBookCommandHandler> _logger; // Lägg till logger

        public UpdateBookCommandHandler(IRepository<Book> repository, ILogger<UpdateBookCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateBookCommand for Book ID: {BookId}", request.UpdatedBook.Id); // Logga när kommandot hanteras

            // Hitta boken med det givna ID:t
            var existingBook = await _repository.GetByIdAsync(request.UpdatedBook.Id, cancellationToken);

            if (existingBook == null)
            {
                _logger.LogWarning("No book found with ID: {BookId}", request.UpdatedBook.Id); // Logga varning om bok inte finns
                throw new KeyNotFoundException($"No book found with ID {request.UpdatedBook.Id}.");
            }

            // Uppdatera bokens egenskaper
            existingBook.Title = request.UpdatedBook.Title;
            existingBook.Description = request.UpdatedBook.Description;
            existingBook.Author = request.UpdatedBook.Author;

            // Logga innan boken uppdateras
            _logger.LogInformation("Updating book with ID: {BookId}. New Title: {Title}, New Description: {Description}, New Author: {Author}",
                                    existingBook.Id, existingBook.Title, existingBook.Description, existingBook.Author);

            // Uppdatera boken i databasen
            await _repository.UpdateAsync(existingBook, cancellationToken);

            // Logga när uppdateringen är klar
            _logger.LogInformation("Successfully updated book with ID: {BookId}", existingBook.Id);

            // Returnera den uppdaterade boken
            return existingBook;
        }
    }
}
