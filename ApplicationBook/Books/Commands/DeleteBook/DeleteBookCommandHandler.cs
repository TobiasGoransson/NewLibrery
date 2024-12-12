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
    namespace ApplicationBook.Books.Commands.DeleteBook
    {
        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<List<Book>>>
        {
            private readonly IRepository<Book> _repository;
            private readonly ILogger<DeleteBookCommandHandler> _logger;

            public DeleteBookCommandHandler(IRepository<Book> repository, ILogger<DeleteBookCommandHandler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<OperationResult<List<Book>>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handling DeleteBookCommand to delete book with ID: {BookId}", request.Id);

                var bookToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

                if (bookToDelete == null)
                {
                    _logger.LogWarning("No book found with ID: {BookId}", request.Id);
                    return OperationResult<List<Book>>.Failure($"No book found with ID {request.Id}.");
                }

                try
                {
                    await _repository.DeleteByIdAsync(request.Id);
                    _logger.LogInformation("Book with ID: {BookId} has been deleted successfully.", request.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deleting book with ID: {BookId}", request.Id);
                    return OperationResult<List<Book>>.Failure("An error occurred while deleting the book.");
                }

                var books = await _repository.GetAllAsync();
                if (books == null || !books.Any())
                {
                    _logger.LogWarning("No books found after deleting book with ID: {BookId}", request.Id);
                    return OperationResult<List<Book>>.Failure("No books found in the repository.");
                }

                _logger.LogInformation("Retrieved updated list of books after deleting book with ID: {BookId}", request.Id);
                return OperationResult<List<Book>>.Successfull(books);
            }
        }
    }
}

