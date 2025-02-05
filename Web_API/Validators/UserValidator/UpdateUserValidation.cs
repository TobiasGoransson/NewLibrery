using Domain;
using FluentValidation;

namespace Web_API.Validators.UserValidator
{
    public class UpdateUserValidation : AbstractValidator<User>

    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
