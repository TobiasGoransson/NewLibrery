using Domain;
using MediatR;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<List<Book>>
    {


        public CreateBookCommand(Book bookToAdd)
        {
            NewBook = bookToAdd;
        }
        public Book NewBook { get; }
    }
}
