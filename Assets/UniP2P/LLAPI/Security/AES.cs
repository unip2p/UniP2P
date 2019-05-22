using System.IO;
using System.Security.Cryptography;
using MessagePack;

namespace UniP2P.LLAPI
{
    public class AES
    {
        const int KEY_SIZE = 128;
        const int BLOCK_SIZE = 128;

        public static (byte[] result, byte[] iv) Encrypt(byte[] value, byte[] key)
        {
            if (key == null)
            {
                return (value, null);
            }
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = KEY_SIZE;
                aes.BlockSize = BLOCK_SIZE;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.GenerateIV();

                byte[] result = aes.CreateEncryptor().TransformFinalBlock(value, 0, value.Length);

                return (result, aes.IV); 
            }
        }

        public static byte[] Decrypt(byte[] value, byte[] key , byte[] iv)
        {
            byte[] result = new byte[value.Length];

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(value))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(result, 0, result.Length);                    
                    }
                }

            }

            return result;
        }
    }
}