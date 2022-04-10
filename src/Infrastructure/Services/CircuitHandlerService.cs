using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace CleanArchitecture.Blazor.Infrastructure.Services;
public class CircuitHandlerService : CircuitHandler
{

    private readonly IUsersStateContainer _usersStateContainer;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private string _circuitId=string.Empty;

    public CircuitHandlerService(IUsersStateContainer usersStateContainer, AuthenticationStateProvider authenticationStateProvider)
    {
        _usersStateContainer = usersStateContainer;
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProvider_AuthenticationStateChanged;  
    }
    ~CircuitHandlerService()
    {
        _authenticationStateProvider.AuthenticationStateChanged -= AuthenticationStateProvider_AuthenticationStateChanged;
    }
    public override async Task OnConnectionUpAsync(Circuit circuit,
        CancellationToken cancellationToken)
    {
        _circuitId = circuit.Id;
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _usersStateContainer.Update(circuit.Id, state.User.Identity?.Name);
    }
    
    public override Task OnConnectionDownAsync(Circuit circuit,
        CancellationToken cancellationToken)
    {
        _usersStateContainer.Remove(circuit.Id);

        return Task.CompletedTask;
    }
    private async void AuthenticationStateProvider_AuthenticationStateChanged(Task<AuthenticationState> state)
    {
        _usersStateContainer.Update(_circuitId, (await state).User.Identity?.Name);
    }
}