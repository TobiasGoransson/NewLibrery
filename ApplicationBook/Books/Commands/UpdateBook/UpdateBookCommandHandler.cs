using ApplicationBook.Books.Commands.UpdateBook;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<Book>>
{
    private readonly IRepository<Book> _repository;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(IRepository<Book> repository, ILogger<UpdateBookCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<Book>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateBookCommand for Book ID: {BookId}", request.UpdatedBook.BId);

        var existingBook = await _repository.GetByIdAsync(request.UpdatedBook.BId, cancellationToken);

        if (existingBook == null)
        {
            _logger.LogWarning("No book found with ID: {BookId}", request.UpdatedBook.BId);
            return OperationResult<Book>.Failure($"No book found with ID {request.UpdatedBook.BId}.");
        }

        existingBook.Title = request.UpdatedBook.Title;
        existingBook.Description = request.UpdatedBook.Description;
        existingBook.Author = request.UpdatedBook.Author;

        _logger.LogInformation("Updating book with ID: {BookId}. New Title: {Title}, New Description: {Description}, New Author: {Author}",
                                existingBook.BId, existingBook.Title, existingBook.Description, existingBook.Author);

        try
        {
            await _repository.UpdateAsync(existingBook, cancellationToken);
            _logger.LogInformation("Successfully updated book with ID: {BookId}", existingBook.BId);
            return OperationResult<Book>.Successfull(existingBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating book with ID: {BookId}", existingBook.BId);
            return OperationResult<Book>.Failure("An error occurred while updating the book.");
        }
    }