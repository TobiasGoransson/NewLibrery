using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Authors.Queries.GetAllAuthors
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
            // Hämta alla författare från repository
            var authors = await _repository.GetAllAsync();
            return authors ?? new List<Author>();

        }
    }
}
