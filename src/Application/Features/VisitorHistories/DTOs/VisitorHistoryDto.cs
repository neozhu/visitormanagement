// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;


public class VisitorHistoryDto:IMapFrom<VisitorHistory>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VisitorHistory, VisitorHistoryDto>()
              .ForMember(x => x.CheckinPoint, s => s.MapFrom(y => $"{y.CheckinPoint.Site.Name} - {y.CheckinPoint.Name}"))
              .ForMember(x => x.Visitor, s => s.MapFrom(y => y.Visitor.Name))
              .ForMember(x => x.Avatar, s => s.MapFrom(y => y.Visitor.Avatar))
              .ForMember(x => x.CompanyName, s => s.MapFrom(y => y.Visitor.CompanyName));
        profile.CreateMap<VisitorHistoryDto, VisitorHistory>(MemberList.None)
              .ForMember(x => x.Visitor, s => s.Ignore())
              .ForMember(x => x.CheckinPoint, s => s.Ignore());
    }
    public int Id { get; set; }
    public int? VisitorId { get; set; }
    public string? Avatar { get; set; }
    public string? Visitor { get; set; }
    public string? CompanyName { get; set; }
    public int? CheckinPointId { get; set; }
    public string? CheckinPoint { get; set; }
    public string? Stage { get; set; }
    public string? Comment { get; set; }
    public DateTime? TransitDateTime { get; set; } = DateTime.Now;
    public decimal? Temperature { get; set; }
    public string? Photo { get; set; }

}

