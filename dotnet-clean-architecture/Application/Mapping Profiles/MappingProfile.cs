using Application.Response.User;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(User), typeof(UserDTO)).ReverseMap();
        }
    }
}