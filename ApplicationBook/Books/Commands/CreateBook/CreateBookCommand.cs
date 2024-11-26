using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommand: IRequest<List<Book>>
    {
        

        public CreateBookCommand(Book bookToAdd)
        {
            NewBook = bookToAdd;
        }
        public Book NewBook { get; }
    }
}
