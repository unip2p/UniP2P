using System;
using System.Security.Cryptography;
using MessagePack;

namespace UniP2P.LLAPI
{
    public class RSA
    {      
        const int keysize = 1024;
        string secretKey;
        
        public RSAPublicKey RequestKey()
        {         
            byte[] publicModules;
            
            byte[] publicExponent;

            using (var rsa = new RSACryptoServiceProvider(keysize))
            {
                try
                {
                    secretKey = rsa.ToXmlString(true);

                    RSAParameters publicParam = rsa.ExportParameters(false);
                    publicModules = publicParam.Modulus;
                    publicExponent = publicParam.Exponent;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.Clear();
                }
            }

            return new RSAPublicKey {Modules = publicModules, Exponent = publicExponent};
        }

        public (byte[] encrypted, byte[] aeskey) CreateKey(byte[] modules, byte[] exponent)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.GenerateKey();
            var password = aes.Key;
            var salt = new byte[24];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var kf = new Rfc2898DeriveBytes(password, salt, 5000);
            byte[] key = kf.GetBytes(128 / 8);

            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider(keysize))
            {
                try
                {
                    var publicParam = new RSAParameters();
                    publicParam.Modulus = modules;
                    publicParam.Exponent = exponent;
                    rsa.ImportParameters(publicParam);

                    encrypted = rsa.Encrypt(key, false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.Clear();
                }
            }

            return (encrypted, key);
        }

        public byte[] AcceptKey(byte[] encrypted)
        {
            byte[] key;
            using (var rsa = new RSACryptoServiceProvider(keysize))
            {
                try
                {
                    rsa.FromXmlString(secretKey);
                    key = rsa.Decrypt(encrypted, false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.Clear();
                }
            }

            return key;
        }

    }

    [MessagePackObject]
    public class RSAPublicKey
    {
        [Key(0)]
        public byte[] Modules;
        [Key(1)]
        public byte[] Exponent;

        public byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }

        public static RSAPublicKey Deserialize(byte[] value)
        {
            return MessagePackSerializer.Deserialize<RSAPublicKey>(value);
        }
    }
}