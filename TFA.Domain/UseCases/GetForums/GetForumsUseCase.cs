using Microsoft.EntityFrameworkCore;
using TFA.Domain.ModelsDTO;
using TFA.Storage;

namespace TFA.Domain.UseCases.GetForums
{
    public class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly ForumDbContext _context;

        public GetForumsUseCase(ForumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ForumDTO>> Execute(CancellationToken cancellationToken) =>       
             await _context.Forums
            .Select(f => new ForumDTO
            {
                Id = f.ForumId,
                Title = f.Title
            })
            .ToArrayAsync();        
    }
}
