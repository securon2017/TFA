using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.Authentication
{
    public interface ISecurityManager
    {
        bool ComparePasswords(string password, string salt, string passwordHash);

        (string Salt, string Hash) GeneratePasswordParts(string password);
    }
}
