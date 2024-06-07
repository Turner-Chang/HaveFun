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

        // 密碼使用Argon2id來做雜湊
        public byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 24; // 平行運算(基本上看核心數)
            argon2.MemorySize = 1024 * 1024 * 2; // 用於計算的記憶體大小，2GB
            argon2.Iterations = 4; // 迭代運算次數

            return argon2.GetBytes(32);
        }
    }
}
