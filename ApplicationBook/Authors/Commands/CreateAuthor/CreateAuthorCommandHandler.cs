
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<AuthorDtoWithId>>
{
    private readonly IRepository<Author> _repository;
    private readonly ILogger<CreateAuthorCommandHandler> _logger;

    public CreateAuthorCommandHandler(IRepository<Author> repository, ILogger<CreateAuthorCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<AuthorDtoWithId>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CreateAuthorCommand for {AuthorName}", request.NewAuthor.FirstName);

        // Map DTO to domain entity
        var author = new Author
        {
            FirstName = request.NewAuthor.FirstName,
            LastName = request.NewAuthor.LastName
        };

        try
        {
            // Save to the repository
            await _repository.CreateAsync(author);

            // Map the domain entity back to a DTO
            var authorDtoWithId = new AuthorDtoWithId(author.AId, author.FirstName, author.LastName);
           

            // Log success
            _logger.LogInformation("Successfully created author with ID {AuthorId}", author);

            return OperationResult<AuthorDtoWithId>.Successfull(authorDtoWithId); // Return AuthorDto instead of Autho
        }
        catch (Exception ex)
        {
            // Log exception
            _logger.LogError(ex, "Error occurred while creating an author.");
            return OperationResult<AuthorDtoWithId>.Failure("An error occurred while creating the author.");
        }
    }
}
