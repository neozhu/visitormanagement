using CleanArchitecture.Blazor.Infrastructure.Extensions;
namespace CleanArchitecture.Blazor.Infrastructure.Services.Authentication;

public class ProfileService 
{
    public event Action? OnChange;
    public UserModel Profile { get; private set; } = new();
    public Task Set(ClaimsPrincipal principal)
    {
        Profile =  new UserModel()
        {
            Avatar = principal.GetProfilePictureDataUrl(),
            DisplayName = principal.GetDisplayName(),
            Email = principal.GetEmail(),
            PhoneNumber = principal.GetPhoneNumber(),
            Site= principal.GetSite(),
            SiteId = principal.GetSiteId(),
            Role = principal.GetRoles().FirstOrDefault(),
            Roles = principal.GetRoles(),
            UserId = principal.GetUserId(),
            UserName = principal.GetUserName(),
            Department = principal.GetDepartment(),
           
        };
        OnChange?.Invoke();
        return Task.CompletedTask;
    }
    public Task Update(UserModel profile)
    {
        Profile = profile;
        OnChange?.Invoke();
        return Task.CompletedTask;
    }

   
}
