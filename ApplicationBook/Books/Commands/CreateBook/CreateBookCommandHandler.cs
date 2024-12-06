using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly IRepository<Book> _repository;
        private readonly ILogger<CreateBookCommandHandler> _logger; // Lägg till logger

        public CreateBookCommandHandler(IRepository<Book> repository, ILogger<CreateBookCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger; // Spara loggern
        }

        public async Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            // Logga när vi börjar hantera kommandot
            _logger.LogInformation("Handling CreateBookCommand to add new book titled: {BookTitle}", request.NewBook.Title);

            // Lägg till den nya boken i repositoryt
            try
            {
                await _repository.CreateAsync(request.NewBook);
                _logger.LogInformation("Book titled '{BookTitle}' successfully added.", request.NewBook.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book titled: {BookTitle}", request.NewBook.Title);
                throw;  // Kasta om undantaget så att det hanteras av den överordnade logiken
            }

            // Hämta och returnera den uppdaterade listan med böcker
            var books = await _repository.GetAllAsync();
            if (books == null || !books.Any())
            {
                _logger.LogWarning("No books found after adding '{BookTitle}'.", request.NewBook.Title);
            }
            else
            {
                _logger.LogInformation("Retrieved updated list of books after adding '{BookTitle}'.", request.NewBook.Title);
            }

            return books ?? new List<Book>();
        }
    }
}
