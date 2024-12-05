using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly IRepository<Book> _repository;

        public CreateBookCommandHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            // Lägg till den nya boken i repositoryt
            await _repository.CreateAsync(request.NewBook);

            // Hämta och returnera den uppdaterade listan med böcker
            var books = await _repository.GetAllAsync();
            return books ?? new List<Book>();
        }
    }

}
