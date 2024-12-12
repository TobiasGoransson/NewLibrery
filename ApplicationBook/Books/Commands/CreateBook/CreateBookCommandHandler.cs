using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<List<Book>>>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IRepository<Book> repository, ILogger<CreateBookCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Book>>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateBookCommand to add new book titled: {BookTitle}", request.NewBook.Title);

            try
            {
                await _repository.CreateAsync(request.NewBook);
                _logger.LogInformation("Book titled '{BookTitle}' successfully added.", request.NewBook.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book titled: {BookTitle}", request.NewBook.Title);
                return OperationResult<List<Book>>.Failure("An error occurred while adding the book.");
            }

            var books = await _repository.GetAllAsync();
            if (books == null || !books.Any())
            {
                _logger.LogWarning("No books found after adding '{BookTitle}'.", request.NewBook.Title);
                return OperationResult<List<Book>>.Failure("No books found in the repository.");
            }

            _logger.LogInformation("Retrieved updated list of books after adding '{BookTitle}'.", request.NewBook.Title);
            return OperationResult<List<Book>>.Successfull(books);
        }
    }
}