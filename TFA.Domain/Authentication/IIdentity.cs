﻿namespace TFA.Domain.Authentication
{
    public interface IIdentity
    {
        public Guid UserId { get; }
    }

    public class User : IIdentity
    {
        public User(Guid userId)
        {
            UserId = userId;
        }

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
