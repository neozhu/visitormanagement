// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;

public static class VisitorHistoryCacheKey
{
    public const string GetAllCacheKey = "all-VisitorHistories";
    public static string GetPagtionCacheKey(string parameters) {
        return $"VisitorHistoriesWithPaginationQuery,{parameters}";
    }
        static VisitorHistoryCacheKey()
    {
        SharedExpiryTokenSource = new CancellationTokenSource(new TimeSpan(3,0,0));
    }
    public static string GetByVisitorIdCacheKey(int? id)
    {
        return $"GetByVisitorIdCacheKey-{id}";
    }
    public static CancellationTokenSource SharedExpiryTokenSource { get; private set; }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(SharedExpiryTokenSource.Token));
}

