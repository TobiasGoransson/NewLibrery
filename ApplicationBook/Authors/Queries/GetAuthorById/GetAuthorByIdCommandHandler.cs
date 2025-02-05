using Microsoft.Extensions.Logging;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IRepository<Author> repository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAuthorByIdQuery for Author ID: {AuthorId}", request.Id);

            // Kontrollera att ID är giltigt
            if (request.Id <= 0)
            {
                _logger.LogWarning("Invalid ID: {AuthorId} provided for GetAuthorByIdQuery", request.Id);
                return OperationResult<Author>.Failure("The ID must be a positive integer.");
            }

            try
            {
                // Hämta författaren från repository
                var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

                // Kontrollera om författaren hittades
                if (author == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} was not found.", request.Id);
                    return OperationResult<Author>.Failure($"Author with ID {request.Id} was not found.");
                }

                _logger.LogInformation("Successfully retrieved Author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Successfull(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
