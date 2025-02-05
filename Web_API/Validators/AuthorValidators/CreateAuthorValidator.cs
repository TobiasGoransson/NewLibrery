using Domain;
using FluentValidation;



namespace Web_API.Validators.AuthorValidators
{
    public class CreateAuthorValidator : AbstractValidator<Author>
    {
        public CreateAuthorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
    
    
}
