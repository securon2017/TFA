using Microsoft.EntityFrameworkCore;
using TFA.Domain.Authentication;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Storage;

namespace TFA.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly ICreateTopicStorage _storage;

        public CreateTopicUseCase(IIdentityProvider identityProvider, ICreateTopicStorage storage)
        {
            _identityProvider = identityProvider;
            _storage = storage;
        }
        public async Task<TopicDTO> Execute(Guid forumId, string title, CancellationToken cancellationToken)
        {
            var forumExists = await _storage.ForumExists(forumId, cancellationToken);
            if(!forumExists) 
            { 
                throw new ForumNotFoundException(forumId); 
            }

            return await _storage.CreateTopic(forumId, _identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
