using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiServer_Example.Domains.DTO
{
    public class FileDto
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
    }
}
