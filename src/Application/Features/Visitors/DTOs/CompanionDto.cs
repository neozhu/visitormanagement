using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
public class CompanionDto : IMapFrom<Companion>
{
    public void Mapping(Profile profile)
    {

        profile.CreateMap<Companion, CompanionDto>()
           .ForMember(x => x.TrackingState,s=> s.Ignore());
     
        profile.CreateMap<CompanionDto, Companion>(MemberList.None);

    }
    public bool Checked { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TripCode { get; set; }
    public string? HealthCode { get; set; }
    public string? QrCode { get; set; }
    public string? NucleicAcidTestReport { get; set; }
    public int? VisitorId { get; set; }
    public string? IdentificationNo { get; set; }
    public DateTime? CheckinDateTime { get; set; }
    public DateTime? CheckoutDateTime { get; set; }
    public TrackingState TrackingState { get; set; } = TrackingState.Unchanged;
}
