using FluentValidation;
using Domain;



namespace Web_API.Validators.BookValidator
{
    public class UpdateBookValidator : AbstractValidator<Book>
    {
        public UpdateBookValidator()
        {
            RuleFor(x => x.BId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.AId).NotEmpty();
        }
    }
}
