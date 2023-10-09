using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        Task<TopicDTO> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
    }
}
