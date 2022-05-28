using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Create;
public class CreateVisitorCompanionDtoValidator : AbstractValidator<CompanionDto>
{
    public CreateVisitorCompanionDtoValidator()
    {
        RuleFor(v => v.Name).MaximumLength(256).NotEmpty();
        RuleFor(v => v.IdentificationNo).MaximumLength(256).NotEmpty()
                     .Matches(new Regex(@"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}$)"));
        RuleFor(v => v.TripCode).NotNull();
        RuleFor(v => v.HealthCode).NotNull();
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CompanionDto>.CreateWithOptions((CompanionDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
