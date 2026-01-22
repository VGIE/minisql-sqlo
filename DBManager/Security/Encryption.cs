using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public static class Encryption
    {
        public static string Encrypt(string value)
        {
            if (value == null)
                return null;

            var md5 = MD5.Create();
            var originalBytes = Encoding.Default.GetBytes(value);
            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }
    }
}
