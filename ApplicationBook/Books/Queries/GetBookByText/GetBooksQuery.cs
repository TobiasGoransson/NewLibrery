using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetBooksQuery : IRequest<OperationResult<List<Book>>>
    {
    }
}

