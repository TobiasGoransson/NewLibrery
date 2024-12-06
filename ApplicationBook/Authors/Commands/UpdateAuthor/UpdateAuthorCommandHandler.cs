using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationBook.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(IRepository<Author> repository, ILogger<UpdateAuthorCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Author> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateAuthorCommand for Author ID: {AuthorId}", request.Id);

            try
            {
                // Hämta författaren från repository
                var author = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (author == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found. Update operation aborted.", request.Id);
                    return null;
                }

                _logger.LogInformation("Author with ID {AuthorId} found. Updating author details...", request.Id);

                // Uppdatera egenskaper
                author.FirstName = request.FirstName;
                author.LastName = request.LastName;

                // Spara uppdateringar
                await _repository.UpdateAsync(author, cancellationToken);
                _logger.LogInformation("Author with ID {AuthorId} has been successfully updated.", request.Id);

                return author; // Returnera den uppdaterade författaren
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the author with ID {AuthorId}.", request.Id);
                throw;
            }
        }
    }
}
