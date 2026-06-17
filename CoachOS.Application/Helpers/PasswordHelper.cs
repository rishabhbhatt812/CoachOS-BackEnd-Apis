using System;
using System.Security.Cryptography;
using System.Text;

namespace CoachOS.Application.Helpers
{
    public static class PasswordHelper
    {
        public static (string Hash, string Salt) CreateHash(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = Convert.ToBase64String(hmac.Key);
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return (hash, salt);
        }

        public static bool VerifyHash(string password, string hash, string salt)
        {
            try
            {
                using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return computedHash == hash;
            }
            catch
            {
                return false;
            }
        }
    }
}
