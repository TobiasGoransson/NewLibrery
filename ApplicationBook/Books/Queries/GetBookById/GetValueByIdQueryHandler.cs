using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBookById
{
    public class GetValueByIdQueryHandler : IRequestHandler<GetValueByIdQuery, Book>
    {
        private readonly IRepository<Book> _repository;

        public GetValueByIdQueryHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<Book> Handle(GetValueByIdQuery request, CancellationToken cancellationToken)
        {
            // Hämta boken baserat på ID
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (book == null)
            {
                throw new KeyNotFoundException($"Ingen bok hittades med ID {request.Id}.");
            }

            return book;
        }
    }


}
