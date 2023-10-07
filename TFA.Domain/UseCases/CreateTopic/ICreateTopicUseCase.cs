using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicUseCase
    {
        Task<TopicDTO> Execute(Guid forumId, string Title, Guid AuthorId, CancellationToken cancellationToken);
    }
}
