using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
