using Domain;
using MediatR;

namespace ApplicationBook.Authors.Commands.UpdateAuthor
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateAuthorCommand : IRequest<Author>
    {
        public int Id { get; }

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(100, ErrorMessage = "FirstName cannot exceed 100 characters.")]
        public string FirstName { get; }

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(100, ErrorMessage = "LastName cannot exceed 100 characters.")]
        public string LastName { get; }

        public UpdateAuthorCommand(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

    }


}
