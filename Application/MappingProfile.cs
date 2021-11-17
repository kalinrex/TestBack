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
            CreateMap<Activity, ActivityResponseDto>()
            .ForPath(adto => adto.Property.Id, a => a.MapFrom(a => a.Property.id))
            .ForPath(adto => adto.Property.Title, a => a.MapFrom(a => a.Property.title))
            .ForPath(adto => adto.Property.Address, a => a.MapFrom(a => a.Property.address))
            .ForPath(adto => adto.Survey.Id, a => a.MapFrom(a => a.Survey.id))
            .ForPath(adto => adto.Survey.Answers, a => a.MapFrom(a => a.Survey.answers))
            .ForPath(adto => adto.Survey.Activity_id, a => a.MapFrom(a => a.Survey.activity_id))
            ;
        }
    }
}
