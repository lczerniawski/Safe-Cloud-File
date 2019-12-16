using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.DTO
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] Modulus { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
