using System;
using System.Security.Cryptography;
using System.Text;

namespace UniP2P.LLAPI
{
    public class HMAC
    {
        public static string GenerateSHA256(string data, string key)
        {
            HMACSHA256 HMACSHA256 = new HMACSHA256();
            HMACSHA256.Key = Encoding.UTF8.GetBytes(key);
            byte[] result = HMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(result);
        }
    }

}