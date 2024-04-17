using FluentValidation;

namespace cwiczenia6;

public class AnimalUpdateValidator : AbstractValidator<UpdateAnimalRequest>
{
    public AnimalUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(200);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Area).NotEmpty().MaximumLength(200);
    }
}