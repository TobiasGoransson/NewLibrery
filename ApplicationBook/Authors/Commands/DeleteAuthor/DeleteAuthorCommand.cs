using Domain;
using MediatR;

namespace ApplicationBook.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<Author>>
    {
        public int Id { get; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
