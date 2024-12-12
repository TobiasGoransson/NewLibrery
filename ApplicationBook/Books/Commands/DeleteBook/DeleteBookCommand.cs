using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<List<Book>>>
    {
        public int Id { get; }
        public DeleteBookCommand(int id)
        {
            Id = id;
        }
    }
}