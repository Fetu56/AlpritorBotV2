using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using System;

namespace AlpritorBotV2.CryptModule
{
    static class CryptAES
    {
        private static readonly ICryptoTransform _encryptor, _decryptor;
        private static readonly UTF8Encoding _encoder;
        private static readonly byte[] _key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["KeyAES"]!);
        private static readonly byte[] _iv = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["IVAES"]!);

        static CryptAES()
        {
            var rm = Aes.Create();
            _encryptor = rm.CreateEncryptor(_key, _iv);
            _decryptor = rm.CreateDecryptor(_key, _iv);
            _encoder = new UTF8Encoding();
        }

        public static string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(_encoder.GetBytes(unencrypted)));
        }

        public static string Decrypt(string encrypted)
        {
            return _encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public static byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, _encryptor);
        }

        public static byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, _decryptor);
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new();
            using (CryptoStream cs = new(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
