using ApplicationBook.Authors.Queries.GetAllAuthors;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<Author>>
{
    private readonly IRepository<Author> _repository;
    private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

    public GetAllAuthorsQueryHandler(IRepository<Author> repository, ILogger<GetAllAuthorsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllAuthorsQuery.");

        try
        {
            // Hämtar alla författare från repository
            var authors = await _repository.GetAllAsync();

            if (authors == null || authors.Count == 0)
            {
                _logger.LogWarning("No authors found in the repository.");
            }
            else
            {
                _logger.LogInformation("Retrieved {AuthorCount} authors from the repository.", authors.Count);
            }

            // Returnera en tom lista om inga författare hittas
            return authors ?? new List<Author>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching authors.");
            throw;
        }
    }
}
