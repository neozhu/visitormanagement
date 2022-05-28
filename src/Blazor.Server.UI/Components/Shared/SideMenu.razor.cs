using Microsoft.AspNetCore.Components;
using MudBlazor;
using Blazor.Server.UI.Models.SideMenu;
using Blazor.Server.UI.Services.Navigation;
using CleanArchitecture.Blazor.Application.Common.Models;
using CleanArchitecture.Blazor.Infrastructure.Constants.Role;
using Microsoft.AspNetCore.Components.Authorization;
using CleanArchitecture.Blazor.Infrastructure.Extensions;

namespace Blazor.Server.UI.Components.Shared;

public partial class SideMenu:IDisposable
{
    private IEnumerable<MenuSectionModel> _menuSections = new List<MenuSectionModel>();
    [EditorRequired] [Parameter] public bool SideMenuDrawerOpen { get; set; } 
    [EditorRequired] [Parameter] public EventCallback<bool> SideMenuDrawerOpenChanged { get; set; }
    [EditorRequired] [Parameter] public UserModel User { get; set; } = default!;

    [Inject] private IMenuService _menuService { get; set; } = default!;
    [CascadingParameter]
    protected Task<AuthenticationState> _authState { get; set; } = default!;
    [Inject]
    private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
        _authenticationStateProvider.AuthenticationStateChanged += _authenticationStateProvider_AuthenticationStateChanged;
        await loadMenu();
    }
    private async Task loadMenu()
    {
        var authstate = await _authState;
        var roles = authstate.User.GetRoles();
        if (roles is null) return;
        if (roles.Contains(RoleConstants.AdministratorRole))
        {
            _menuSections = _menuService.AllFeatures;
        }
        else if (roles.Contains(RoleConstants.UserRole))
        {
            _menuSections = _menuService.UserFeatures;
        }
        else if (roles.Contains(RoleConstants.GuestRole))
        {
            _menuSections = _menuService.GuestFeatures;
        }
        else if (roles.Contains(RoleConstants.GuardRole))
        {
            _menuSections = _menuService.GuardFeatures;
        }
    }
    private async void _authenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
       await loadMenu();
    }

    public void Dispose()
    {
        _authenticationStateProvider.AuthenticationStateChanged -= _authenticationStateProvider_AuthenticationStateChanged;
    }
}