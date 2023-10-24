using Microsoft.Extensions.Options;
using System.Security.Cryptography;


namespace TFA.Domain.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationStorage storage;
        private readonly ISecurityManager securityManager;
        private readonly AuthenticationConfiguration configuration;
        private readonly Lazy<TripleDES> tripDesService = new(TripleDES.Create);

        public AuthenticationService(
            IAuthenticationStorage storage,
            ISecurityManager securityManager,
            IOptions<AuthenticationConfiguration> options)
        {
            this.storage = storage;
            this.securityManager = securityManager;
            configuration = options.Value;
        }

        public async Task<(bool success, string authToken)> SignIn(BasicSignInCredentionals credentionals, CancellationToken cancellationToken)
        {
            var recognizedUser = await storage.FindUser(credentionals.Login, cancellationToken);
            if (recognizedUser is null)
            {
                throw new Exception("User not found");
            }

            var success = securityManager.ComparePasswords(credentionals.Password, recognizedUser.Salt, recognizedUser.PasswordHash);
            var userIdBytes = recognizedUser.UserId.ToByteArray();

            using var encriptedStream = new MemoryStream();
            var key = Convert.FromBase64String(configuration.Key);
            var iv = Convert.FromBase64String(configuration.Iv);
            await using (var stream = new CryptoStream(
                encriptedStream,
                tripDesService.Value.CreateEncryptor(key, iv),
                CryptoStreamMode.Write))
            {
                await stream.WriteAsync(userIdBytes, cancellationToken);
            }


            return (success, Convert.ToBase64String(encriptedStream.ToArray()));
        }

        public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
        {
            using var decriptedStream = new MemoryStream();
            var key = Convert.FromBase64String(configuration.Key);
            var iv = Convert.FromBase64String(configuration.Iv);

            await using (var stream = new CryptoStream(
                decriptedStream,
                tripDesService.Value.CreateDecryptor(key, iv),
                CryptoStreamMode.Write))
            {
                var encryptedBytes = Convert.FromBase64String(authToken);
                await stream.WriteAsync(encryptedBytes, cancellationToken);
            }
            var userId = new Guid(decriptedStream.ToArray());

            return new User(userId);
        }

    }
}
