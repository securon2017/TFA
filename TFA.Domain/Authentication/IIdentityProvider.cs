namespace TFA.Domain.Authentication
{
    public interface IIdentityProvider
    {
        public IIdentity Current { get; }
    }
}
