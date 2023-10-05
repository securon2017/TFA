namespace TFA.Storage
{
    public class Topic
    {
        public Guid TopicId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid UserId { get; set; }

        public Guid ForumId { get; set; }

        public string Title { get; set; }
    }
}