using ApplicationBook.Common; // För OperationResult
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<Author>>
    {
        private readonly IRepository<Author> _repository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(IRepository<Author> repository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult<Author>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteAuthorCommand for Author ID: {AuthorId}", request.Id);

            try
            {
                // Hämta författaren baserat på ID
                var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

                if (author == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found. Delete operation aborted.", request.Id);
                    return OperationResult<Author>.Failure($"Author with ID {request.Id} not found.");
                }

                // Ta bort författaren från repository
                await _repository.DeleteByIdAsync(request.Id);
                _logger.LogInformation("Author with ID {AuthorId} has been deleted.", request.Id);

                // Returnera framgång med den borttagna författaren
                return OperationResult<Author>.Successfull(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author with ID {AuthorId}.", request.Id);
                return OperationResult<Author>.Failure("An error occurred while deleting the author.");
            }
        }
    }
}
