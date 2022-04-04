// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Devices.Commands.AddEdit;

public class AddEditDeviceCommandValidator : AbstractValidator<AddEditDeviceCommand>
{
    public AddEditDeviceCommandValidator()
    {
           RuleFor(v => v.Name)
                 .MaximumLength(256)
                 .NotEmpty();
         
     }
     public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
     {
        var result = await ValidateAsync(ValidationContext<AddEditDeviceCommand>.CreateWithOptions((AddEditDeviceCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
     };
}

