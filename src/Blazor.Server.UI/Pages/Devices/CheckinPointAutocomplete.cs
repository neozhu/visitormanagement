using CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;
using CleanArchitecture.Blazor.Application.Features.CheckinPoints.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Blazor.Server.UI.Pages.Devices;

public class CheckinPointAutocomplete : MudAutocomplete<int?>
{

    [Inject]
    private ISender _mediator { get; set; } = default!;


    private List<CheckinPointDto> _checkinpoints = new();

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
            _checkinpoints = (await _mediator.Send(new GetAllCheckinPointsQuery())).ToList();
            ForceRender(true);
        }
    }

    private Task<IEnumerable<int?>> Search(string value)
    {
        var list = new List<int?>();
        if (string.IsNullOrEmpty(value))
        {
            foreach (var i in _checkinpoints.Select(x=>x.Id))
            {
                list.Add(i);
            }
        }
        else
        {
            var result = _checkinpoints.Where(x =>value.Contains(x.Name)).Select(x => x.Id);
            foreach (var i in result)
            {
                list.Add(i);
            }
        }
        return Task.FromResult(list.AsEnumerable());
    }

    private string GetName(int? id) {
        var chpoint = _checkinpoints.Find(b => b.Id == id);
        if (chpoint is null)
        {
            return String.Empty;
        }
        else
        {
            return $"{chpoint.Site} - {chpoint.Name}";
        }
    }
}