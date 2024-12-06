using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Author>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(IRepository<Author> repository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Author> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            // Hämta författaren baserat på ID
            var author = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _logger.LogInformation("Handling DeleteAuthorCommand for Author ID: {AuthorId}", request.Id);

            try
            {
                
                if (author != null)
                {
                    _logger.LogInformation("Author with ID {AuthorId} found. Deleting author...", request.Id);

                    // Ta bort författaren från repository
                    await _repository.DeleteByIdAsync(request.Id);
                    _logger.LogInformation("Author with ID {AuthorId} has been deleted.", request.Id);
                }
                else
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found. Delete operation aborted.", request.Id);
                }

                // Returnera den borttagna författaren (eller null om inte funnen)
                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author with ID {AuthorId}.", request.Id);
                throw;
            }
        }
    }
}

