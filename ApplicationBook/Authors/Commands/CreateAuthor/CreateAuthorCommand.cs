using Domain;
using Domain.Dtos;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace ApplicationBook.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<OperationResult<AuthorDtoWithId>>
    {
        
       

        public CreateAuthorCommand(AuthorDto newAuthor)
        {
            NewAuthor = newAuthor;
        }
        public AuthorDto NewAuthor { get; }
    }
}