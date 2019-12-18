using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiServer_Example.Domains.DTO
{
    public class FileCreateDto
    {
        [Required]
        public string FileName { get; set; }
        [Required] 
        public string FileType { get; set; }
        [Required]
        public IFormFile FormFile { get; set; }
        [Required]
        public bool IsShared { get; set; }
        public string JsonFileId { get; set; }
    }
}
