using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Blazor.Server.UI.Pages.Employees;

public class DepartmentAutocomplete : MudAutocomplete<int?>
{

    [Inject]
    private ISender _mediator { get; set; } = default!;


    private List<DepartmentDto> _departments = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {


        Dense = true;
        ResetValueOnEmptyText = true;
        SearchFunc = Search;
        ToStringFunc = GetName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one brand to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _departments = (await _mediator.Send(new GetAllDepartmentsQuery())).ToList();
            ForceRender(true);
        }
    }

    private Task<IEnumerable<int?>> Search(string value)
    {
        var list = new List<int?>();
        if (string.IsNullOrEmpty(value))
        {
            var result = _departments.Select(x => x.Id);
            foreach (var i in result)
            {
                list.Add(i);
            }
        }
        else
        {
            var result = _departments.Where(x => x.Name.ToLower().Contains(value.ToLower())).Select(x => x.Id);
            foreach (var i in result)
            {
                list.Add(i);
            }
        }
        return Task.FromResult(list.AsEnumerable());
    }

    private string GetName(int? id) => _departments.Find(b => b.Id == id)?.Name ?? string.Empty;
}