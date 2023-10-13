using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.GetTopics
{
    public interface IGetTopicsStorage
    {
        Task<(IEnumerable<TopicDTO> resources, int totalCount)> GetTopics(
            Guid forumId, int skip, int take, CancellationToken cancellationToken);
    }
}
