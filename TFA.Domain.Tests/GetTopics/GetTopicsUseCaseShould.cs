using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsUseCaseShould
    {
        private readonly GetTopicsUseCase sut;
        private readonly Mock<IGetTopicsStorage> storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<TopicDTO> resources, int totalCount)>> getTopicsSetup;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<ForumDTO>>> getForumsSetup;

        public GetTopicsUseCaseShould()
        {
            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator.Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            Mock<IGetForumsStorage> getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            storage = new Mock<IGetTopicsStorage>();
            getTopicsSetup = storage.Setup(
                s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));


            sut = new GetTopicsUseCase(validator.Object, getForumsStorage.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowForumNotGoundException_WhenNoForum()
        {
            var forumId = Guid.Parse("01D45AF3-343C-4C12-9AA2-66D5ADCA93D9");

            getForumsSetup.ReturnsAsync(new ForumDTO[] { new() { Id = Guid.Parse("1985BD42-3300-4EB2-AE8B-A634B184152E") } });

            var query = new GetTopicsQuery(forumId, 0, 1);
            await sut.Invoking(s => s.Execute(query, CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();

        }

        [Fact]
        public async Task ReturnTopics_ExtractedFromStorage_WhenForumExist()
        {
            var forumId = Guid.Parse("DB8BF4E9-D12C-4793-92E1-710172D4746F");

            getForumsSetup.ReturnsAsync(new ForumDTO[] { new() { Id = Guid.Parse("DB8BF4E9-D12C-4793-92E1-710172D4746F") } });
            var expectedResources = new TopicDTO[] { new TopicDTO() };
            var expectedTotalCount = 6;
            getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

            var (actualResources, actualTotalCount) = await sut.Execute(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

            actualResources.Should().BeEquivalentTo(expectedResources);
            actualTotalCount.Should().Be(actualTotalCount);

            storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
