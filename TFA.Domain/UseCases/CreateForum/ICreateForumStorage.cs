using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.CreateForum
{
    public interface ICreateForumStorage
    {
        Task<ForumDTO> Create(string title, CancellationToken cancellationToken);
    }
}
