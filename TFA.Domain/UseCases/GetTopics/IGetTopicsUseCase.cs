using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.GetTopics
{
    public interface IGetTopicsUseCase
    {
        Task<(IEnumerable<TopicDTO> resources, int totalCount)> Execute(
            GetTopicsQuery query, CancellationToken cancellationToken   );
    }
}
