using ApplicationBook.Dtos;
using Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace ApplicationBook.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<Author>
    {
        
       

        public CreateAuthorCommand(AuthorDto newAuthor)
        {
            NewAuthor = newAuthor;
        }
        public AuthorDto NewAuthor { get; }
    }
}