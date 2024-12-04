using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public UpdateBookCommand(Book updatedBook)
        {
            UpdatedBook = updatedBook;
        }

        public Book UpdatedBook { get; }
    }

}
