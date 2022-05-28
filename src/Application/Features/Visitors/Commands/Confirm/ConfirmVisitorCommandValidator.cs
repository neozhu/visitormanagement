// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Approve;

public class ConfirmVisitorCommandValidator : AbstractValidator<ConfirmVisitorCommand>
{
    public ConfirmVisitorCommandValidator()
    {
        RuleFor(x => x.VisitorId).NotEmpty();
    }

}

