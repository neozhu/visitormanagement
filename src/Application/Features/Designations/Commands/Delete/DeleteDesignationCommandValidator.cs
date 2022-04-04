// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Designations.Commands.Delete;

public class DeleteDesignationCommandValidator : AbstractValidator<DeleteDesignationCommand>
{
    public DeleteDesignationCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


