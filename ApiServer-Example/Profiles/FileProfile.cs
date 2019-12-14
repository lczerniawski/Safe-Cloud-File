using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
using ApiServer_Example.Domains.Models;
using AutoMapper;

namespace ApiServer_Example.Profiles
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<FileModel, FileDto>();
        }
    }
}
