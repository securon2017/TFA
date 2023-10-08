using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.GetForums
{
    public interface IGetForumsStorage
    {
        Task<IEnumerable<ForumDTO>> GetForums(CancellationToken cancellationToken);
    }
}
