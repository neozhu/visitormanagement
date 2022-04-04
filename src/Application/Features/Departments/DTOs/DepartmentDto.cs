// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.DTOs;


public class DepartmentDto:IMapFrom<Department>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Department, DepartmentDto>().ReverseMap();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }

}

