using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DesktopApp_Example.DTO;

namespace DesktopApp_Example.Helpers
{
    public static class RSAKeysExtensionsMethods
    {
        public static RSAParameters MapToRsaParameters(this RSAKeys rsaKeys)
        {
            return new RSAParameters
            {
                D = rsaKeys.D,
                DP = rsaKeys.DP,
                DQ = rsaKeys.DQ,
                Exponent = rsaKeys.Exponent,
                InverseQ = rsaKeys.InverseQ,
                Modulus = rsaKeys.Modulus,
                P = rsaKeys.P,
                Q = rsaKeys.Q
            };
        }
    }
}
