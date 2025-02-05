using ApplicationBook.Books.Queries.GetBook;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllValuesQueryHandler : IRequestHandler<GetBooksQuery, OperationResult<List<Book>>>
{
    private readonly IRepository<Book> _repository;
    private readonly ILogger<GetAllValuesQueryHandler> _logger;

    public GetAllValuesQueryHandler(IRepository<Book> repository, ILogger<GetAllValuesQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<List<Book>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetBooksQuery for retrieving all books");

        try
        {
            var books = await _repository.GetAllAsync();

            if (books == null || books.Count == 0)
            {
                _logger.LogWarning("No books found in the repository.");
                return OperationResult<List<Book>>.Failure("No books found in the repository.");
            }

            _logger.LogInformation("Successfully retrieved {BookCount} books from the repository.", books.Count);
            return OperationResult<List<Book>>.Successfull(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving books from the repository.");
            return OperationResult<List<Book>>.Failure("An error occurred while retrieving books.");
        }
    }
}