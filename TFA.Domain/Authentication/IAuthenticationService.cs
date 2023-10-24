namespace TFA.Domain.Authentication
{
    public interface IAuthenticationService
    {
        Task<(bool success, string authToken)> SignIn(BasicSignInCredentionals credentionals, CancellationToken cancellationToken);
        Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);
    }
}
