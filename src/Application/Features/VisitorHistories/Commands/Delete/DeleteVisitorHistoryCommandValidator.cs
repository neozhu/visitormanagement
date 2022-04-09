// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Commands.Delete;

public class DeleteVisitorHistoryCommandValidator : AbstractValidator<DeleteVisitorHistoryCommand>
{
        public DeleteVisitorHistoryCommandValidator()
        {
     
           RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
           throw new System.NotImplementedException();
        }
}
    

