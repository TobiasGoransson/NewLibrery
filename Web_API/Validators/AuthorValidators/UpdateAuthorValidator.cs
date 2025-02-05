using Domain;
using FluentValidation;

namespace Web_API.Validators.AuthorValidators
{
    public class UpdateAuthorValidator : AbstractValidator<Author>
    {
        public UpdateAuthorValidator()
        {
            RuleFor(x => x.AId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
           
        }
    }
}
