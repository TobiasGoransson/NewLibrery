

using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IRepository<Author> repository, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllAuthorsQuery.");

            try
            {
                // Hämta alla författare från repository
                var authors = await _repository.GetAllAsync();

                if (authors == null || authors.Count == 0)
                {
                    _logger.LogWarning("No authors found.");
                    return OperationResult<List<Author>>.Failure("No authors found.");
                }

                _logger.LogInformation("Found {AuthorCount} authors.", authors.Count);
                return OperationResult<List<Author>>.Successfull(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching authors.");
                return OperationResult<List<Author>>.Failure("An error occurred while fetching authors.");
            }
        }
    }
}
