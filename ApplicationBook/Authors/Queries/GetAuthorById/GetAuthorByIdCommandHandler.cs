using Microsoft.Extensions.Logging;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;  // Lägg till logger

        public GetAuthorByIdQueryHandler(IRepository<Author> repository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;  // Spara loggern
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAuthorByIdQuery for Author ID: {AuthorId}", request.Id); // Logga när metoden startar

            // Kontrollera att ID är giltigt
            if (request.Id <= 0)
            {
                _logger.LogWarning("Invalid ID: {AuthorId} provided for GetAuthorByIdQuery", request.Id);  // Logga om ID är ogiltigt
                throw new ArgumentException("The ID must be a positive integer.");
            }

            // Hämta författaren från repository
            var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

            // Kontrollera om författaren hittades
            if (author == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} was not found.", request.Id);  // Logga om författaren inte hittades
                throw new KeyNotFoundException($"Author with ID {request.Id} was not found.");
            }

            _logger.LogInformation("Successfully retrieved Author with ID: {AuthorId}", request.Id); // Logga om författaren hittades

            return author;
        }
    }
}
