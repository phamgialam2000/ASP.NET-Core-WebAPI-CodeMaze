﻿using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                    .ForMember(c => c.FullAddress,
                    opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();
            //CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            //Identity
            CreateMap<UserForRegistrationDto, User>();

        }

    }
}
