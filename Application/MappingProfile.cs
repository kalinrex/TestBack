using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyDto>();
        }
    }
}
