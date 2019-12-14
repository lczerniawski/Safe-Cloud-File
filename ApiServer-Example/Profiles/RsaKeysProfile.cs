using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;
using AutoMapper;

namespace ApiServer_Example.Profiles
{
    public class RsaKeysProfile : Profile
    {
        public RsaKeysProfile()
        {
            CreateMap<RSAParameters, RSAKeys>().ReverseMap();
        }
    }
}
