using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace HaveFun.Common
{
    public class PasswordSecure
    {
        // 產生鹽，回傳byte[]陣列
        public byte[] CreateSalt()
        {
            byte[] buffer = new byte[16];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        // 密碼使用SHA256來做雜湊
        public string HashPassword(string password, byte[] salt)
        {
            using (var hash = SHA256.Create())
            {
                byte[] passwordByte = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[salt.Length + passwordByte.Length];

                // 把passwordByte跟salt放到saltedPassword內
                Buffer.BlockCopy(passwordByte, 0, saltedPassword, 0, passwordByte.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordByte.Length, salt.Length);

                // 把saltedPassword利用SHA256做雜湊
                byte[] hashedPasswordBytes = hash.ComputeHash(saltedPassword);

                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }
    }
}
