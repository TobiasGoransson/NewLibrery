using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetAllValuesQueryHandler : IRequestHandler<GetAllValuesQuery, List<Book>>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<GetAllValuesQueryHandler> _logger; // Lägg till logger

        public GetAllValuesQueryHandler(IRepository<Book> repository, ILogger<GetAllValuesQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<List<Book>> Handle(GetAllValuesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllValuesQuery for retrieving all books"); // Logga när förfrågan hanteras

            // Hämta alla böcker från repositoryn
            var books = await _repository.GetAllAsync();

            if (books == null || books.Count == 0)
            {
                _logger.LogWarning("No books found in the repository."); // Logga varning om inga böcker hittas
            }
            else
            {
                _logger.LogInformation("Successfully retrieved {BookCount} books from the repository.", books.Count); // Logga antal böcker som hämtades
            }

            return books;
        }
    }
}
