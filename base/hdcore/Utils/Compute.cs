using System;
using System.Security.Cryptography;
using System.Text;

namespace hdcore.Utils
{
    public class Compute
    {
        public static string ComputeMd5Hash(string input)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(input);
            HashAlgorithm md5Hasher = MD5.Create();
            return BitConverter.ToString(md5Hasher.ComputeHash(bytes));
        }
    }
}