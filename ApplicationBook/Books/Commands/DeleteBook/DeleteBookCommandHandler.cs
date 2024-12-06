using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, List<Book>>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<DeleteBookCommandHandler> _logger; // Lägg till logger

        public DeleteBookCommandHandler(IRepository<Book> repository, ILogger<DeleteBookCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<List<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // Logga när vi börjar hantera kommandot
            _logger.LogInformation("Handling DeleteBookCommand to delete book with ID: {BookId}", request.Id);

            // Hämta boken som ska tas bort
            var bookToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            // Kontrollera om boken finns
            if (bookToDelete == null)
            {
                _logger.LogWarning("No book found with ID: {BookId}", request.Id); // Logga varning om bok inte finns
                throw new KeyNotFoundException($"No book found with ID {request.Id}.");
            }

            // Ta bort boken
            await _repository.DeleteByIdAsync(request.Id);
            _logger.LogInformation("Book with ID: {BookId} has been deleted successfully.", request.Id); // Logga framgång

            // Hämta och returnera den uppdaterade listan med böcker
            var books = await _repository.GetAllAsync();
            if (books == null || !books.Any())
            {
                _logger.LogWarning("No books found after deleting book with ID: {BookId}", request.Id); // Logga varning om inga böcker återstår
            }
            else
            {
                _logger.LogInformation("Retrieved updated list of books after deleting book with ID: {BookId}", request.Id); // Logga när uppdaterad lista hämtas
            }

            return books.ToList();
        }
    }
}

