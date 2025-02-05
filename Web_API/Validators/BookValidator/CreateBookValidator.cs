using Domain;
using FluentValidation;


namespace Web_API.Validators.BookValidator
{
    public class CreateBookValidator : AbstractValidator<Book>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.AId).NotEmpty();
        }
    }
}
