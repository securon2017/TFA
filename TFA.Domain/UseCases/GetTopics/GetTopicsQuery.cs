using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.UseCases.GetTopics
{
    public record GetTopicsQuery(Guid ForumId, int Skip, int Take);
}
