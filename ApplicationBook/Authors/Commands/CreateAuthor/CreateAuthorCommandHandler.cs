using System.Threading;
using System.Threading.Tasks;
using ApplicationBook.Authors.Commands.CreateAuthor;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationBook.Dtos;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Author>
{
    private readonly IRepository<Author> _Repository;
    private readonly ILogger<CreateAuthorCommandHandler> _logger;
   

    // Konstruktor med ILogger-injektion
    public CreateAuthorCommandHandler(IRepository<Author> eposetory, ILogger<CreateAuthorCommandHandler> logger )
    {
        _logger = logger;
        _Repository = _Repository;
    }

    public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
       
            // Log start of the process
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
            await _Repository.CreateAsync(author);

            // Log success
            _logger.LogInformation("Successfully created author with ID {AuthorId}", author);

            return author;
        }
        catch (Exception ex)
        {
            // Log exception
            _logger.LogError(ex, "Error occurred while creating an author.");
            throw;
        }
    }
}
