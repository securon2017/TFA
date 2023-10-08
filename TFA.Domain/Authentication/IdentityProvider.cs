using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authentication
{
    public class IdentityProvider : IIdentityProvider
    {
        public IIdentity Current => new User(Guid.Parse("22F445AC-30D0-469F-A98D-7775DE2265DF"));
    }
}
