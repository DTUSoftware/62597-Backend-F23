using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ShopBackend.Security
{
    //Build with inspiration from: https://www.youtube.com/watch?v=vspPrnZgSAc&ab_channel=RemigiuszZalewski
    public class PasswordAuth : IPasswordAuth
    {
        private const int saltByteSize = 128 / 8;
        private const int keyByteSize = 256 / 8;
        private int iterationCount = 10000;
        private static readonly HashAlgorithmName _algorithName = HashAlgorithmName.SHA256;

        public bool VerifyPassword(string userPassword, string passwordToVerify)
        {
            var splitPassword = userPassword.Split(';');
            var passwordSalt = Convert.FromBase64String(splitPassword[0]);
            var userPasswordHash = Convert.FromBase64String(splitPassword[1]);

            var passwordHashToVerify = Rfc2898DeriveBytes.Pbkdf2(passwordToVerify, passwordSalt, iterationCount, _algorithName, keyByteSize);
            
            return CryptographicOperations.FixedTimeEquals(userPasswordHash, passwordHashToVerify);
        }

        public string GeneratePasswordHash(string password)
        {
            var passwordSalt = RandomNumberGenerator.GetBytes(saltByteSize);
            var passwordHash = Rfc2898DeriveBytes.Pbkdf2(password, passwordSalt, iterationCount, _algorithName, keyByteSize);

            return string.Join(';',Convert.ToBase64String(passwordSalt), Convert.ToBase64String(passwordHash));
        }

        public bool IsPasswordStrong(string password)
        {
            Regex validPassword = new("^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-/*_]).{12,}$");
            return validPassword.IsMatch(password);
        }

        public bool IsEmailValid(string email)
        {
            Regex validEmail = new("^(.+)@(.+)$");
            return validEmail.IsMatch(email);
        }
    }
}
