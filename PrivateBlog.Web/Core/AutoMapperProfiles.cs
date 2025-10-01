using AutoMapper;
using UnidadResidencial.Web.Models;
using UnidadResidencial.Web.DTOs;

namespace UnidadResidencial.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, SectionDTO>().ReverseMap();
        }
    }
}