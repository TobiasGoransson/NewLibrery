using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
