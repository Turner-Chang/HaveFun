using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HaveFun.Common
{
    public class DESSecure
    {
        private readonly byte[] _rgbKey;
        private readonly byte[] _rgbIV;

        public DESSecure(IConfiguration configuration)
        {
            _rgbKey = Encoding.ASCII.GetBytes(configuration["Des:Key"].Substring(0, 8));
            _rgbIV = Encoding.ASCII.GetBytes(configuration["Des:IV"].Substring(0, 8));
        }

        // 輸入加密的值，回傳加密結果
        public string Encrypt(string text)
        {
            using (var des = DES.Create())
            using (var memStream = new MemoryStream())
            using (var crypStream = new CryptoStream(memStream, des.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write))
            using (var sWriter = new StreamWriter(crypStream))
            {
                sWriter.Write(text);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                return Convert.ToBase64String(memStream.ToArray());
            }
        }

        // 輸入加密的文字，解密成原文
        public string Decrypt(string encryptedText)
        {
            using (var des = DES.Create())
            {
                byte[] buffer = Convert.FromBase64String(encryptedText);

                using (var memStream = new MemoryStream())
                using (var crypStream = new CryptoStream(memStream, des.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write))
                {
                    crypStream.Write(buffer, 0, buffer.Length);
                    crypStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(memStream.ToArray());
                }
            }
        }
    }
}
