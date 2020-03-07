using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer_Example.Domains.DTO
{
    public class UserDto
    {
        public string Email { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] Modulus { get; set; }
    }
}
