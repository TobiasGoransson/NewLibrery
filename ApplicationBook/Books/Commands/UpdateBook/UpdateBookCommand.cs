using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public Book UpdatedBook { get; set; }

        public UpdateBookCommand(Book updatedBook)
        {
            UpdatedBook = updatedBook;
        }
    }


}
