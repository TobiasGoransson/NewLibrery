using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Common; // Add this namespace
using ApplicationBook.Dtos;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<Author>>
{
    private readonly IRepository<Author> _repository;
    private readonly ILogger<CreateAuthorCommandHandler> _logger;

    public CreateAuthorCommandHandler(IRepository<Author> repository, ILogger<CreateAuthorCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<Author>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CreateAuthorCommand for {AuthorName}", request.NewAuthor.FirstName);

        // Map DTO to domain entity
        var author = new Author
        {
            AId = 0,
            FirstName = request.NewAuthor.FirstName,
            LastName = request.NewAuthor.LastName
        };

        try
        {
            // Save to the repository
            await _repository.CreateAsync(author);

            // Log success
            _logger.LogInformation("Successfully created author with ID {AuthorId}", author.AId);

            return OperationResult<Author>.Successfull(author);
        }
        catch (Exception ex)
        {
            // Log exception
            _logger.LogError(ex, "Error occurred while creating an author.");
            return OperationResult<Author>.Failure("An error occurred while creating the author.");
        }
    }
}
