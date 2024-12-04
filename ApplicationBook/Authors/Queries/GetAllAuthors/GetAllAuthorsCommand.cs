using Domain;
using MediatR;

namespace ApplicationBook.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<List<Author>> { }

}
