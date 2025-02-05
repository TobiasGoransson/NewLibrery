using Domain;
using FluentValidation;


namespace Web_API.Validators.UserValidator
{
    public class CreateUserValidator : AbstractValidator<User>
    {
        public CreateUserValidator()
        {

            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
