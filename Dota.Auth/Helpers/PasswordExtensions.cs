using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dota.Auth.Helpers
{
    public static class PasswordExtensions
    {
        /// <summary>
        /// Converts password to hashcode, so it will store in database
        /// as hashcode, but not in usual string
        /// </summary>
        /// <param name="password">string from client</param>
        /// <returns>string hashcode</returns>
        public static string PasswordToStringHashCode(this string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            return string.Join("", bytes.Select(x => x.ToString("x2")));
        }
    }
}