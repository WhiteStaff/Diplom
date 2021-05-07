using System;
using System.Text;

namespace Common.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(this string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}