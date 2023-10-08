namespace TFA.API.Models
{
    public class TopicViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
