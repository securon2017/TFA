using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;

namespace TFA.Domain.Tests.Authentication
{
    public class AuthenticationServiseShould
    {
        private readonly AuthenticationService sut;
        private readonly Mock<IAuthenticationStorage> storage;
        private readonly ISetup<IAuthenticationStorage, Task<RecognizedUser>> findUserSetup;
        private readonly Mock<IOptions<AuthenticationConfiguration>> options;

        public AuthenticationServiseShould()
        {
            storage = new Mock<IAuthenticationStorage>();
            findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var securityManager = new Mock<ISecurityManager>();
            securityManager
                .Setup(m => m.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            options = new Mock<IOptions<AuthenticationConfiguration>>();
            options.Setup(o => o.Value)
                .Returns(new AuthenticationConfiguration
                {
                    Key = "QkEeenXpHqgP6tOWwpUetAFvUUZiMb4f",
                    Iv = "dtEzMsz2ogg="
                });
            sut = new AuthenticationService(storage.Object, securityManager.Object, options.Object);
        }

        [Fact]
        public async Task ReturnSuccess_WhenUserFound()
        {
            findUserSetup.ReturnsAsync(new RecognizedUser
            {
                Salt = "k+sEGR/LrzqX5AtpUdE8wQ==",
                PasswordHash = "n0QA3pc9GgdjQelRXed68rW0h3APNMduwSVEMLub+O17XgSfhF8DlzXIvjqDvMtqVsg=",
                UserId = Guid.Parse("F499FDFD-1C18-4C88-8DAF-F1316E446EC6")
            });

            var (success, authToken) = await sut.SignIn(new BasicSignInCredentionals("User", "Password"), CancellationToken.None);
            success.Should().BeTrue();
            authToken.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AuthenticateUser_WhenTheySignIn()
        {
            var userId = Guid.Parse("E317DB4E-3348-422E-B01C-F5651621DE4B");
            findUserSetup.ReturnsAsync(new RecognizedUser
            {
                UserId = userId
            });

            var (success, authToken) = await sut.SignIn(new BasicSignInCredentionals("User", "Password"), CancellationToken.None);
            IIdentity identity = await sut.Authenticate(authToken, CancellationToken.None);
            identity.UserId.Should().Be(userId);
        }
    }
}
