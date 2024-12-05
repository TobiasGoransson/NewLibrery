using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IRepository<Author> _repository;

        public GetAuthorByIdQueryHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            // Kontrollera att ID är giltigt
            if (request.Id <= 0)
            {
                throw new ArgumentException("The ID must be a positive integer.");
            }

            // Hämta författaren från repository
            var author = await _repository.GetByIdAsync(request.Id,cancellationToken);

            // Kontrollera om författaren hittades
            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} was not found.");
            }

            return author;
        }
    }


}
