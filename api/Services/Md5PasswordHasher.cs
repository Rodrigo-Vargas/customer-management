using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class Md5PasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            // That is a very simnple and insecure hash verification. Made just for study purposes

            var md5ProvidedPassword = GetMD5Hash(providedPassword);

            return hashedPassword == md5ProvidedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed; 
        }

        public override string HashPassword(TUser user, string password)
        {
            return GetMD5Hash(password);
        }

        public static string GetMD5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                return Convert.ToBase64String(bytes);
            }
        }
    }
}
