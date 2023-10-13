using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IValidator<CreateTopicCommand> _validator;
        private readonly IIntentionManager _intentionManager;
        private readonly IIdentityProvider _identityProvider;
        private readonly IGetForumsStorage _forumsStorage;
        private readonly ICreateTopicStorage _storage;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IGetForumsStorage forumsStorage,
            ICreateTopicStorage storage)
        {
            _validator = validator;
            _intentionManager = intentionManager;
            _identityProvider = identityProvider;
            _forumsStorage = forumsStorage;
            _storage = storage;
        }
        public async Task<TopicDTO> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);

            var (forumId, title) = command;

            _intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await _forumsStorage.ThrowIfForumNotFound(forumId, cancellationToken);

            return await _storage.CreateTopic(forumId, _identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
