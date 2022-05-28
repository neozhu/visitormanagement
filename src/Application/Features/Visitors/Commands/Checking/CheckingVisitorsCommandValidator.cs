// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Checking;

public class CheckingVisitorsCommandValidator : AbstractValidator<CheckingVisitorsCommand>
{
    public CheckingVisitorsCommandValidator()
    {
        RuleFor(x => x.VisitorId).NotEmpty();
        RuleFor(x => x.Outcome).NotEmpty();
    }

}

