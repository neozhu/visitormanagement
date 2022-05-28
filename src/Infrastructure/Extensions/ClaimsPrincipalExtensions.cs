// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CleanArchitecture.Blazor.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Email);


    public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
       => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
       => claimsPrincipal.FindFirstValue(ClaimTypes.Name);
    public static string GetSite(this ClaimsPrincipal claimsPrincipal)
      => claimsPrincipal.FindFirstValue(ClaimTypes.Locality);
    public static int? GetSiteId(this ClaimsPrincipal claimsPrincipal)
      => claimsPrincipal.Claims.Any(x=>x.Type ==ApplicationClaimTypes.SiteId) ? Convert.ToInt32(claimsPrincipal.FindFirstValue(ApplicationClaimTypes.SiteId)):null;
    public static string GetDisplayName(this ClaimsPrincipal claimsPrincipal)
         => claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
    public static string GetProfilePictureDataUrl(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ApplicationClaimTypes.ProfilePictureDataUrl);

    public static string GetDepartment(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ApplicationClaimTypes.Department);
    public static string[] GetRoles(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();

    public static void SetDisplayName (this ClaimsPrincipal claimsPrincipal, string displayName)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if(identity is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim( ClaimTypes.GivenName, displayName ));
        }
        
    }
    public static void SetProfilePictureUrl(this ClaimsPrincipal claimsPrincipal, string url)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ApplicationClaimTypes.ProfilePictureDataUrl);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(ApplicationClaimTypes.ProfilePictureDataUrl, url));
        }
    }
    public static void SetPhoneNumber(this ClaimsPrincipal claimsPrincipal, string phoneNumber)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, phoneNumber));
        }
    }
    public static void SetSite(this ClaimsPrincipal claimsPrincipal, string site)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Locality);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(ClaimTypes.Locality, site));
        }
    }
    public static void SetSiteId(this ClaimsPrincipal claimsPrincipal, int? siteId)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is not null && siteId is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Locality);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(ApplicationClaimTypes.SiteId, siteId.ToString()));
        }
    }
    public static void SetDepartment(this ClaimsPrincipal claimsPrincipal, string deparment)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claim = identity.Claims.FirstOrDefault(x => x.Type == ApplicationClaimTypes.Department);
            if (claim is not null)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(ApplicationClaimTypes.Department, deparment));
        }
    }
}

