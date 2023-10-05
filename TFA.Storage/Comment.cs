namespace TFA.Storage
{
    public class Comment
    {
        public Guid CommentId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid TopicId { get; set; }

        public Guid UserId { get; set; }

        public string Text { get; set; }
    }
}
