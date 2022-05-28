using Blazor.Server.UI.Models.SideMenu;

namespace Blazor.Server.UI.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> AllFeatures { get; }
    IEnumerable<MenuSectionModel> UserFeatures { get; }
    IEnumerable<MenuSectionModel> GuestFeatures { get; }
    IEnumerable<MenuSectionModel> GuardFeatures { get; }
}
