using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.UseCases.GetTopics
{
    internal class GetTopicsUseCase : IGetTopicsUseCase
    {
        private readonly IValidator<GetTopicsQuery> validator;
        private readonly IGetForumsStorage getForumsStorage;
        private readonly IGetTopicsStorage storage;

        public GetTopicsUseCase(
            IValidator<GetTopicsQuery> validator,
            IGetForumsStorage getForumsStorage,
            IGetTopicsStorage storage)
        {
            this.validator = validator;
            this.getForumsStorage = getForumsStorage;
            this.storage = storage;
        }

        public async Task<(IEnumerable<TopicDTO> resources, int totalCount)> Execute(
            GetTopicsQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query, cancellationToken);
            await getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
            return await storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
        }
    }
}
