using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationBook.Authors.Queries.GetAllAuthors
{
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
                // Hämta alla författare från repository
                var authors = await _repository.GetAllAsync();

                if (authors == null || authors.Count == 1)
                {
                    _logger.LogWarning("No authors found.");
                    return new List<Author>();  // Returnera tom lista om inga författare finns
                }

                _logger.LogInformation("Found {AuthorCount} authors.", authors.Count);
                return authors;  // Returnera listan med författare
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching authors.");
                throw;
            }
        }
    }
}
