using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetAllValuesQuery : IRequest<List<Book>>
    {
    }

}
