using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System;

namespace AlpritorBotV2.CryptModule
{
    static class CryptAES
    {
        private static ICryptoTransform encryptor, decryptor;
        private static UTF8Encoding encoder;
        private static readonly byte[] _key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["KeyAES"]!);
        private static readonly byte[] _iv = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["IVAES"]!);

        static CryptAES()
        {
            var rm = Aes.Create();
            encryptor = rm.CreateEncryptor(_key, _iv);
            decryptor = rm.CreateDecryptor(_key, _iv);
            encoder = new UTF8Encoding();
        }

        public static string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public static string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public static byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public static byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
