using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Blazor.Server.UI.Pages.Identity.Users;

public class AssignDepartmentAutocomplete : MudAutocomplete<string?>
{

    [Inject]
    private ISender _mediator { get; set; } = default!;


    private List<DepartmentDto> _list = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Dense = true;
        SearchFunc = Search;
        ToStringFunc = GetName;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one brand to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _list = (await _mediator.Send(new GetAllDepartmentsQuery())).ToList();
            ForceRender(true);
        }
    }

    private Task<IEnumerable<string?>> Search(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Task.FromResult(_list.Select(x => x.Name).ToList().AsEnumerable());
        }
        else
        {
            var result = _list.Where(x => x.Name.StartsWith(value)).Select(x => x.Name).ToList();
            return Task.FromResult(result.AsEnumerable());
        }
    }

    private string GetName(string? txt)
    {
        if (string.IsNullOrEmpty(txt))
        {
            return String.Empty;
        }
        else
        {
            return _list.Find(b => b.Name == txt)?.Name;
        }
    }
}