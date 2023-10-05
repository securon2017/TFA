using System.ComponentModel.DataAnnotations;

namespace TFA.Storage
{
    public class User
    {
        public Guid UserId { get; set; }

        [MaxLength(20)]
        public string Login { get; set; }

    }
}