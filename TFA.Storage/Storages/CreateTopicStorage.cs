using Microsoft.EntityFrameworkCore;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Storage.Storages
{
    internal class CreateTopicStorage : ICreateTopicStorage
    {
        private readonly IGuidFactory _guidFactory;
        private readonly IMomentProvider _momentProvider;
        private readonly ForumDbContext _dbContext;

        public CreateTopicStorage(
            IGuidFactory guidFactory,
            IMomentProvider momentProvider,
            ForumDbContext dbContext)
        {
            _guidFactory = guidFactory;
            _momentProvider = momentProvider;
            _dbContext = dbContext;
        }

        public async Task<TopicDTO> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken)
        {
            var topicId = _guidFactory.Create();
            var topic = new Topic
            {
                TopicId = topicId,
                ForumId = forumId,
                UserId = userId,
                Title = title,
                CreatedAt = _momentProvider.Now
            };
            await _dbContext.Topics.AddAsync(topic, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return await _dbContext.Topics.Where(t => t.TopicId == topicId)
                .Select(t => new TopicDTO
                {
                    Id = t.TopicId,
                    ForumId = t.ForumId,
                    UserId = t.UserId,
                    Title = t.Title,
                    CreatedAt = t.CreatedAt
                }).FirstAsync(cancellationToken);
        }

        public async Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken) =>
            await _dbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);

    }
}
