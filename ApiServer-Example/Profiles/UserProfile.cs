using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
using ApiServer_Example.Domains.Models;
using AutoMapper;

namespace ApiServer_Example.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
