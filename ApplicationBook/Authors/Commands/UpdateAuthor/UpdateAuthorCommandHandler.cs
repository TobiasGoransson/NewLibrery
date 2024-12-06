using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
    {
        private readonly IRepository<Author> _repository;

        public UpdateAuthorCommandHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            // Hämta författaren från repository
            var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (author != null)
            {
                // Uppdatera egenskaper
                author.FirstName = request.FirstName;
                author.LastName = request.LastName;

                // Spara uppdateringar
                await _repository.UpdateAsync( author,cancellationToken);
            }

            return author; // Returnera den uppdaterade författaren (eller null om inte funnen)
        }
    }

}
