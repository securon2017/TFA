using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper mapper;

        public GetForumsStorage(
            IMemoryCache memoryCache,
            ForumDbContext dbContext,
            IMapper mapper)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ForumDTO>> GetForums(CancellationToken cancellationToken)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _memoryCache.GetOrCreateAsync<ForumDTO[]>(
                nameof(GetForums),
            entry =>
            {
                return _dbContext.Forums
                .ProjectTo<ForumDTO>(mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
            });
#pragma warning restore CS8603 // Possible null reference return.


        }
    }
}
