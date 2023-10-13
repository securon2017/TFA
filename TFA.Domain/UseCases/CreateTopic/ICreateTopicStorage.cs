using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage
    {
        Task<TopicDTO> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
    }
}
