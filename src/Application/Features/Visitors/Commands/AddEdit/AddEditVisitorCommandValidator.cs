// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.AddEdit;

public class AddEditVisitorCommandValidator : AbstractValidator<AddEditVisitorCommand>
{
    public AddEditVisitorCommandValidator()
    {

        RuleFor(v => v.Name).MaximumLength(256).NotEmpty();
        RuleFor(v => v.PassCode).MaximumLength(256).NotEmpty();
        RuleFor(v => v.LicensePlateNumber).MaximumLength(50)
                      .Matches(new Regex(@"^(([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z](([0-9]{5}[DF])|([DF]([A-HJ-NP-Z0-9])[0-9]{4})))|([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳使领]))$"))
                      .When(x=>!string.IsNullOrEmpty(x.LicensePlateNumber));
        RuleFor(v => v.Email).MaximumLength(256).EmailAddress().When(x=>!string.IsNullOrEmpty(x.Email));
        RuleFor(v => v.IdentificationNo).MaximumLength(256).NotEmpty()
                     .Matches(new Regex(@"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}$)"))
                     .When(x => !string.IsNullOrEmpty(x.IdentificationNo));
        RuleFor(v => v.PhoneNumber).NotEmpty().NotNull()
                      .MinimumLength(8)
                      .MaximumLength(20)
                      .Matches(new Regex(@"^1(3\d|4[5-9]|5[0-35-9]|6[2567]|7[0-8]|8\d|9[0-35-9])\d{8}$"));
        RuleFor(v => v.Purpose).MaximumLength(256).NotEmpty();
        RuleFor(v => v.EmployeeId).NotNull();
        //RuleFor(v => v.Avatar).NotNull();
        //RuleFor(v => v.TripCode).NotNull();
        //RuleFor(v => v.HealthCode).NotNull();
        RuleFor(v => v.PrivacyPolicy).Equal(true);
        RuleFor(v => v.Promise).Equal(true);
        RuleFor(v => v.ExpectedDate).NotNull().GreaterThan(DateTime.Now.Date);
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AddEditVisitorCommand>.CreateWithOptions((AddEditVisitorCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

