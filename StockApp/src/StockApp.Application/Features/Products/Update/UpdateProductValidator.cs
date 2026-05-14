using FluentValidation;

namespace StockApp.Application.Features.Products.Update;

public class UpdateProductValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0);
    }
}
