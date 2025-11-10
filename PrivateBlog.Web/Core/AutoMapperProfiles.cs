using AutoMapper;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.DTOs;
using UnidadResidencial.Web.Models;

namespace UnidadResidencial.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Section, SectionDTO>().ReverseMap();

            CreateMap<Residencial, ResidencialDTO>().ReverseMap();

            CreateMap<User, AccountUserDTO>().ReverseMap();

            CreateMap<Permission, PermissionDTO>();

            CreateMap<ResidencialRole, ResidencialRoleDTO>().ReverseMap();
        }
    }
}