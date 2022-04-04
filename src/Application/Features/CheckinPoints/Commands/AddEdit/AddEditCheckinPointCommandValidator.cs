// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.Commands.AddEdit;

public class AddEditCheckinPointCommandValidator : AbstractValidator<AddEditCheckinPointCommand>
{
    public AddEditCheckinPointCommandValidator()
    {
           RuleFor(v => v.Name)
               .MaximumLength(256)
               .NotEmpty();
        RuleFor(v => v.SiteId).NotNull().GreaterThan(0);
              

    }
     public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
     {
        var result = await ValidateAsync(ValidationContext<AddEditCheckinPointCommand>.CreateWithOptions((AddEditCheckinPointCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
     };
}

