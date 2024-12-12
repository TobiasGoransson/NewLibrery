using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Queries.GetBookById
{
    public class GetValueByIdQueryHandler : IRequestHandler<GetValueByIdQuery, OperationResult<Book>>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<GetValueByIdQueryHandler> _logger; // Lägg till logger

        public GetValueByIdQueryHandler(IRepository<Book> repository, ILogger<GetValueByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<OperationResult< Book>> Handle(GetValueByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetValueByIdQuery for Book ID: {BookId}", request.Id); // Logga när förfrågan hanteras

            // Hämta boken baserat på ID
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (book == null)
            {
                _logger.LogWarning("No book found with ID: {BookId}", request.Id); // Logga varning om bok inte hittas
                
                return OperationResult<Book>.Failure($"No book found with ID {request.Id}.");
            }

            _logger.LogInformation("Book with ID: {BookId} found successfully", request.Id); // Logga att boken hittades

            return OperationResult<Book>.Successfull(book);
        }
    }
}
