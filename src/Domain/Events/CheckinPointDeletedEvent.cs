// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Domain.Events;

    public class CheckinPointDeletedEvent : DomainEvent
    {
        public CheckinPointDeletedEvent(CheckinPoint item)
        {
            Item = item;
        }

        public CheckinPoint Item { get; }
    }

