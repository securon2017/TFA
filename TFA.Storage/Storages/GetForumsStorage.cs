using Microsoft.EntityFrameworkCore;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages
{
    public class GetForumsStorage : IGetForumsStorage
    {
        private readonly ForumDbContext _dbContext;

        public GetForumsStorage(ForumDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ForumDTO>> GetForums(CancellationToken cancellationToken)
        {
            return await _dbContext.Forums
                .Select(f => new ForumDTO
                {
                    Id = f.ForumId,
                    Title = f.Title
                }).ToArrayAsync(cancellationToken);
        }
    }
}
