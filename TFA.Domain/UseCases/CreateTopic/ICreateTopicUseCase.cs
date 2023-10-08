using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        Task<TopicDTO> Execute(Guid forumId, string Title, CancellationToken cancellationToken);
    }
}
