namespace TFA.Domain.Authentication
{
    public interface IIdentity
    {
        public Guid UserId { get; }
    }

    internal class User : IIdentity
    {
        public User(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }

    internal static class IdentityExtensions
    {
        public static bool IsAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}
