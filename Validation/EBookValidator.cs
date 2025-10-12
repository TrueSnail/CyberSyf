using E_Book_Store.Models;
using FluentValidation;

namespace E_Book_Store.Validation;

public class EBookValidator : AbstractValidator<EBook>
{
    public EBookValidator()
    {
        const string GUID_REGEX = "^(?:\\{{0,1}(?:[0-9a-fA-F]){8}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){12}\\}{0,1})$";

        RuleFor(x => x.Id).NotNull().NotEmpty().Matches(GUID_REGEX);
        RuleFor(x => x.Author).NotNull().NotEmpty();
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
