// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Devices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;


public class CheckinPointDto:IMapFrom<CheckinPoint>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CheckinPoint, CheckinPointDto>()
               .ForMember(x => x.Devices, s => s.MapFrom(y => y.Devices.Select(x => new DeviceDto() { Name = x.Name, Status = x.Status, IPAddress = x.IPAddress }).ToList()))
               .ForMember(x => x.Site, s => s.MapFrom(y => y.Site.Name))
               .ForMember(x => x.Address, s => s.MapFrom(y => y.Site.Address));
        profile.CreateMap<CheckinPointDto, CheckinPoint>()
               .ForMember(x => x.Devices, opt => opt.Ignore())
               .ForMember(x => x.Site, opt => opt.Ignore());
    }
                       
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<DeviceDto> Devices { get; set; } = new List<DeviceDto>();
    public int? SiteId { get; set; }
    public string? Site { get; set; }
    public string? Address { get; set; }

}

