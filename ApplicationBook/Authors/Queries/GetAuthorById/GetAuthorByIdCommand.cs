using Domain;
using MediatR;

namespace ApplicationBook.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<Author>
    {
        public int Id { get; }

        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
