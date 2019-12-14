using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer_Example.Domains.Models
{
    public class RSAKeys
    {
        [Required]
        public byte[] D { get; set; }
        [Required]
        public byte[] DP { get; set; }
        [Required]
        public byte[] DQ { get; set; }
        [Required]
        public byte[] Exponent { get; set; }
        [Required]
        public byte[] InverseQ { get; set; }
        [Required]
        public byte[] Modulus { get; set; }
        [Required]
        public byte[] P { get; set; }
        [Required]
        public byte[] Q { get; set; }
    }
}
