// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

public static class VisitorCacheKey
{
    public const string GetAllCacheKey = "all-Visitors";
    public static string GetPagtionCacheKey(string parameters) {
        return $"VisitorsWithPaginationQuery,{parameters}";
    }
    public static string Search(string keyword)
    {
        return $"SearchVisitorQuery:{keyword}";
    }
    static VisitorCacheKey()
    {
        SharedExpiryTokenSource = new CancellationTokenSource(new TimeSpan(3,0,0));
    }
    public static CancellationTokenSource SharedExpiryTokenSource { get; private set; }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(SharedExpiryTokenSource.Token));
}

