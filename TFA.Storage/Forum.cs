using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    [Table("forums")]
    public class Forum
    {
        [Key]        
        public Guid ForumId { get; set; }

        public string Title { get; set; }

        [InverseProperty(nameof(Topic.Forum))]
        ICollection<Topic> Topics { get; set;}

    }
}