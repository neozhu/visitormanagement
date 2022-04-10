// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Create;

public class ApprovalVisitorsCommandValidator : AbstractValidator<ApprovalVisitorsCommand>
{
    public ApprovalVisitorsCommandValidator()
    {
        RuleFor(x => x.VisitorId).NotEmpty();
        RuleFor(x => x.Outcome).NotEmpty();
    }

}

