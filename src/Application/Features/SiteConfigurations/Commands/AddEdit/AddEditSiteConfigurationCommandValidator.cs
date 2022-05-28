// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Commands.AddEdit;

public class AddEditSiteConfigurationCommandValidator : AbstractValidator<AddEditSiteConfigurationCommand>
{
    public AddEditSiteConfigurationCommandValidator()
    {
       RuleFor(v => v.SiteId)
                    .NotEmpty();
    }
     public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
     {
        var result = await ValidateAsync(ValidationContext<AddEditSiteConfigurationCommand>.CreateWithOptions((AddEditSiteConfigurationCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
     };
}

