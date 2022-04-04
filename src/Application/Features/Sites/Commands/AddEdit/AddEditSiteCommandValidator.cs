// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Sites.Commands.AddEdit;

public class AddEditSiteCommandValidator : AbstractValidator<AddEditSiteCommand>
{
    public AddEditSiteCommandValidator()
    {
              RuleFor(v => v.Name)
                .MaximumLength(256)
                 .NotEmpty();
         
     }
     public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
     {
        var result = await ValidateAsync(ValidationContext<AddEditSiteCommand>.CreateWithOptions((AddEditSiteCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
     };
}

