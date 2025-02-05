using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<OperationResult<Book>>
    {
        public int Id { get; }

        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }

}
