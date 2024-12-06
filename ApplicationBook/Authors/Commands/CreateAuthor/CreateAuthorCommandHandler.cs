using ApplicationBook.Authors.Queries.GetAllAuthors;
using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Authors.Commands.CreateAuthor
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<Author>>
    {
        private readonly IRepository<Author> _repository;

        public GetAllAuthorsQueryHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<List<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            // Hämtar alla författare från repository
            var authors = await _repository.GetAllAsync();

            // Returnera en tom lista om inga författare hittas
            return authors ?? new List<Author>();
        }
    }

}
