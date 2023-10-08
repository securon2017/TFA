using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using System.Security.Principal;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;

namespace TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _sut;
        private readonly Mock<ICreateTopicStorage> storage;
        private readonly ISetup<ICreateTopicStorage, Task<bool>> forumExistSetup;
        private readonly ISetup<ICreateTopicStorage, Task<TopicDTO>> createTopicSetup;
        private readonly ISetup<Authentication.IIdentity, Guid> getCurrentUserIdSetup;
        private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;
        private readonly Mock<IIntentionManager> intentionManager;

        public CreateTopicUseCaseShould()
        {
            storage = new Mock<ICreateTopicStorage>();
            forumExistSetup = storage.Setup(x => x.ForumExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            createTopicSetup = storage.Setup(x => 
                x.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var identity = new Mock<Authentication.IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            getCurrentUserIdSetup = identity.Setup(p => p.UserId);

            intentionManager = new Mock<IIntentionManager>();
            intentionIsAllowedSetup = intentionManager.Setup(p => p.IsAllowed(It.IsAny<TopicIntention>()));

            _sut = new CreateTopicUseCase(intentionManager.Object, identityProvider.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("87029B64-8551-4E4E-9F14-8945CC4098CD");
            intentionIsAllowedSetup.Returns(false);
            await _sut.Invoking(s => s.Execute(forumId, "whatever", CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {

            var forumId = Guid.Parse("4646944A-E718-46B2-A082-5DC498FCA487");

            intentionIsAllowedSetup.Returns(true);
            forumExistSetup.ReturnsAsync(false);

            await _sut.Invoking(s => s.Execute(forumId, "Some Title", CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();

            storage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
        {
            var forumId = Guid.Parse("DD3D80A9-4CEC-4886-8B93-F0BF8782009F");
            var userId = Guid.Parse("5FBD486C-1F9F-4832-B48F-53BE8A2E0E72");

            intentionIsAllowedSetup.Returns(true);
            forumExistSetup.ReturnsAsync(true);
            getCurrentUserIdSetup.Returns(userId);
            var expected = new TopicDTO();
            createTopicSetup.ReturnsAsync(expected);

            var actual = await _sut.Execute(forumId, "Hello world!", CancellationToken.None);
            actual.Should().Be(expected);

            storage.Verify(s => 
                s.CreateTopic(forumId, userId, "Hello world!", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}