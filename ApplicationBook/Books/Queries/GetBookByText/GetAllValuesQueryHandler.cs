using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetAllValuesQueryHandler : IRequestHandler<GetAllValuesQuery, List<Book>>
    {
        private readonly IRepository<Book> _repository;

        public GetAllValuesQueryHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<List<Book>> Handle(GetAllValuesQuery request, CancellationToken cancellationToken)
        {
            // Hämta alla böcker från repositoryn
            var books = await _repository.GetAllAsync();

            return books;
        }
    }

}
