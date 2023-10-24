using System.Security.Cryptography;
using System.Text;

namespace TFA.Domain.Authentication
{
    public class SecurityManager : ISecurityManager
    {
        private readonly Lazy<SHA256> sha256 = new(SHA256.Create());
        public bool ComparePasswords(string password, string salt, string passwordHash)
        {
            var newHash = ComputeSha(password, salt);
            return string.Equals(
                Encoding.UTF8.GetString(newHash),
                Encoding.UTF8.GetString(Convert.FromBase64String(passwordHash)));
        }

        public (string Salt, string Hash) GeneratePasswordParts(string password)
        {
            const int saltLenght = 100;
            var buffer = RandomNumberGenerator.GetBytes(saltLenght * 4 / 3);
            var base64string = Convert.ToBase64String(buffer);
            var salt = base64string.Length > saltLenght
                ? base64string[..saltLenght]
                : base64string;

            var hash = ComputeSha(password, salt);
            return (salt, Convert.ToBase64String(hash));
        }

        private byte[] ComputeSha(string plainText, string salt)
        {
            var buffer = Encoding.UTF8.GetBytes(plainText).Concat(Convert.FromBase64String(salt)).ToArray();
            lock (sha256)
            {
                return sha256.Value.ComputeHash(buffer);
            }
        }
    }
}
