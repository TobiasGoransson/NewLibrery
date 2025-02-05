using Domain;
using Domain.Dtos;
using MediatR;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<OperationResult<BookDto>>
    {
        public CreateBookCommand(BookDto newbook)
        {
            NewBook = newbook;
        }
        public BookDto NewBook { get; }
    }
}