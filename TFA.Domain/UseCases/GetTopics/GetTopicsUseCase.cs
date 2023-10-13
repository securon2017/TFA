using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.GetTopics
{
    internal class GetTopicsUseCase : IGetTopicsUseCase
    {
        private readonly IValidator<GetTopicsQuery> validator;
        private readonly IGetTopicsStorage storage;

        public GetTopicsUseCase(
            IValidator<GetTopicsQuery> validator,
            IGetTopicsStorage storage)
        {
            this.validator = validator;
            this.storage = storage;
        }

        public async Task<(IEnumerable<TopicDTO> resources, int totalCount)> Execute(
            GetTopicsQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query, cancellationToken);
            return await storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
        }
    }
}
