using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer_Example.Domains.Models
{
    [Table("File")]
    public class FileModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public string FileType { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public bool IsShared { get; set; }
        public string JsonFileId { get; set; }
    }
}
