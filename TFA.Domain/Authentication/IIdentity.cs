namespace TFA.Domain.Authentication
{
    public interface IIdentity
    {
        public Guid UserId { get; }
    }

    public static class IdentityExtensions
    {
        public static bool IsAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}
