using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        public Task<TopicDTO> Execute(Guid forumId, string Title, Guid AuthorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
