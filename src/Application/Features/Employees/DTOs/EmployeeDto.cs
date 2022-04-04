// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Employees.DTOs;


public class EmployeeDto:IMapFrom<Employee>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Employee, EmployeeDto>()
           .ForMember(x => x.Designation, s => s.MapFrom(y => y.Designation.Name))
           .ForMember(x => x.Department, s => s.MapFrom(y => y.Department.Name));
        profile.CreateMap<EmployeeDto, Employee>(MemberList.None);
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? DepartmentId { get; set; }
    public string? Department { get; set; }
    public int? DesignationId { get; set; }
    public string? Designation { get; set; }
    public string? About { get; set; }
    public string? Avatar { get; set; }

}

