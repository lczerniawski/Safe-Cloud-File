using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp_Example.DTO
{
    public class SenderPublicKey
    {
        public SenderPublicKey(byte[] expontent, byte[] modulus)
        {
            Expontent = expontent;
            Modulus = modulus;
        }

        public byte[] Expontent { get; }
        public byte[] Modulus { get; }
    }
}
