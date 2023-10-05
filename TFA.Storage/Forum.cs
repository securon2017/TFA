using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class Forum
    {
        public Guid TopicId { get; set; }

        public string Title { get; set; }

        [InverseProperty(nameof(Topic.Forum))]
        ICollection<Topic> Topics { get; set;}

    }
}