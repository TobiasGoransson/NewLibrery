using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, List<Book>>
    {
        private readonly IRepository<Book> _repository;

        public DeleteBookCommandHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<List<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // Hämta boken som ska tas bort
            var bookToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            // Kontrollera om boken finns
            if (bookToDelete == null)
            {
                throw new KeyNotFoundException($"Ingen bok hittades med ID {request.Id}.");
            }

            // Ta bort boken
            await _repository.DeleteByIdAsync(request.Id);

            // Hämta och returnera den uppdaterade listan med böcker
            var books = await _repository.GetAllAsync();
            return books.ToList();
        }
    }


}
