using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<List<Book>>
    {
        public int bookId { get; }
        public DeleteBookCommand(int Id)
        {
            Id = bookId;
        }

    }

}
