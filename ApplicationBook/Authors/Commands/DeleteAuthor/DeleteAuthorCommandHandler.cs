using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;

using MediatR;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Author>
    {
        private readonly IRepository<Author> _repository;

        public DeleteAuthorCommandHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            // Hämta författaren baserat på ID
            var author = await _repository.GetByIdAsync(request.Id,cancellationToken);
            if (author != null)
            {
                // Ta bort författaren från repository
                await _repository.DeleteByIdAsync(request.Id);
            }

            // Returnera den borttagna författaren (eller null om inte funnen)
            return author;
        }
    }

}
