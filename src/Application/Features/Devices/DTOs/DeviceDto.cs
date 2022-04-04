// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Devices.DTOs;


public class DeviceDto:IMapFrom<Device>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Device, DeviceDto>()
               .ForMember(x => x.CheckinPoint, s => s.MapFrom(y => y.CheckinPoint.Name))
               .ForMember(x => x.Site, s => s.MapFrom(y => y.CheckinPoint.Site.Name));

        profile.CreateMap<DeviceDto, Device>()
               .ForMember(x => x.CheckinPoint, opt => opt.Ignore());
     
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IPAddress { get; set; }
    public string? Parameters { get; set; }
    public string? Status { get; set; }
    public int? CheckinPointId { get; set; }
    public string? CheckinPoint { get; set; }

    public string? Site { get; set; }

}

