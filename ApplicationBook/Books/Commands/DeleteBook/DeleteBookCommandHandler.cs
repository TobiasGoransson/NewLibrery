using ApplicationBook.Books.Commands.CreateBook;
using Domain;
using MediatR;
using Infrastructur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructur.Database;

namespace ApplicationBook.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, List<Book>>
    {
        private readonly FakeDatabase fakeDatabase;

        public DeleteBookCommandHandler(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Task<List<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            
            var bookToDelete = fakeDatabase.Books.FirstOrDefault(b => b.Id == request.bookId);

            if (bookToDelete == null)
            {
                throw new KeyNotFoundException($"Ingen bok hittades med ID {request.bookId}.");
            }

            
            fakeDatabase.Books.Remove(bookToDelete);

            
            return Task.FromResult(fakeDatabase.Books);
        }
    }



}
