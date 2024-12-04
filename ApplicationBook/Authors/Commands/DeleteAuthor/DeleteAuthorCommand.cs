using Domain;
using MediatR;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<Author>
    {
        public int Id { get; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
