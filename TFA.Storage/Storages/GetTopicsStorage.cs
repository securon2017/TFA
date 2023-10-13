using Microsoft.EntityFrameworkCore;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Storage.Storages
{
    internal class GetTopicsStorage : IGetTopicsStorage
    {
        private readonly ForumDbContext dbContext;

        public GetTopicsStorage(ForumDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<(IEnumerable<TopicDTO> resources, int totalCount)> GetTopics(
            Guid forumId, int skip, int take, CancellationToken cancellationToken)
        {
            var query = dbContext.Topics.Where(t => t.ForumId == forumId);
            var totalCount = await query.CountAsync(cancellationToken);

            var resources = await query
                .Select(t => new TopicDTO
                {
                    Id = t.TopicId,
                    ForumId = t.ForumId,
                    UserId = t.UserId,
                    Title = t.Title,
                    CreatedAt = t.CreatedAt
                })
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken);

            return (resources, totalCount);
        }
    }
}
