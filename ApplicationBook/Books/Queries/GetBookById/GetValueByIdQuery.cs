using Domain;
using MediatR;

namespace ApplicationBook.Books.Queries.GetBookById
{
    public class GetValueByIdQuery : IRequest<OperationResult<Book>>
    {
        public int Id { get; }

        public GetValueByIdQuery(int id)
        {
            Id = id;
        }
    }

}
