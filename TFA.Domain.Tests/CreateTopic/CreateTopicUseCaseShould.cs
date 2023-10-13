using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.Tests.GetTopic
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _sut;
        private readonly Mock<ICreateTopicStorage> storage;
        private readonly ISetup<ICreateTopicStorage, Task<TopicDTO>> createTopicSetup;
        private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
        private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;
        private readonly Mock<IIntentionManager> intentionManager;
        private readonly Mock<IGetForumsStorage> getForumsStorage;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<ForumDTO>>> getForumsSetup;

        public CreateTopicUseCaseShould()
        {
            storage = new Mock<ICreateTopicStorage>();
            createTopicSetup = storage.Setup(x =>
                x.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            var identity = new Mock<IIdentity>();
            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider.Setup(p => p.Current).Returns(identity.Object);
            getCurrentUserIdSetup = identity.Setup(p => p.UserId);

            intentionManager = new Mock<IIntentionManager>();
            intentionIsAllowedSetup = intentionManager.Setup(p => p.IsAllowed(It.IsAny<TopicIntention>()));

            var validator = new Mock<IValidator<CreateTopicCommand>>();
            validator.Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _sut = new CreateTopicUseCase(
                validator.Object, intentionManager.Object, identityProvider.Object, getForumsStorage.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
        {
            var forumId = Guid.Parse("87029B64-8551-4E4E-9F14-8945CC4098CD");
            intentionIsAllowedSetup.Returns(false);
            await _sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "whatever"), CancellationToken.None))
                .Should().ThrowAsync<IntentionManagerException>();
            intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
        }

        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {

            var forumId = Guid.Parse("4646944A-E718-46B2-A082-5DC498FCA487");

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(Array.Empty<ForumDTO>());

            await _sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some Title"), CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();
        }

        [Fact]
        public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
        {
            var forumId = Guid.Parse("DD3D80A9-4CEC-4886-8B93-F0BF8782009F");
            var userId = Guid.Parse("5FBD486C-1F9F-4832-B48F-53BE8A2E0E72");

            intentionIsAllowedSetup.Returns(true);
            getForumsSetup.ReturnsAsync(new ForumDTO[] { new() { Id = forumId } });
            getCurrentUserIdSetup.Returns(userId);
            var expected = new TopicDTO();
            createTopicSetup.ReturnsAsync(expected);

            var actual = await _sut.Execute(new CreateTopicCommand(forumId, "Hello world!"), CancellationToken.None);
            actual.Should().Be(expected);

            storage.Verify(s =>
                s.CreateTopic(forumId, userId, "Hello world!", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}