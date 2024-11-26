using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
