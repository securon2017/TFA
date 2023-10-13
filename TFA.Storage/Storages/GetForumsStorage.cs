using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages
{
    internal class GetForumsStorage : IGetForumsStorage
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ForumDbContext _dbContext;

        public GetForumsStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ForumDTO>> GetForums(CancellationToken cancellationToken)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _memoryCache.GetOrCreateAsync<ForumDTO[]>(
                nameof(GetForums),
                entry =>
            {
                return _dbContext.Forums
                .Select(f => new ForumDTO
                {
                    Id = f.ForumId,
                    Title = f.Title
                }).ToArrayAsync(cancellationToken);
            });
#pragma warning restore CS8603 // Possible null reference return.


        }
    }
}
