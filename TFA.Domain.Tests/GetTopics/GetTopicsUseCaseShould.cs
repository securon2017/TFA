using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsUseCaseShould
    {
        private readonly GetTopicsUseCase sut;
        private readonly Mock<IGetTopicsStorage> storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<TopicDTO> resources, int totalCount)>> getTopicsSetup;

        public GetTopicsUseCaseShould()
        {
            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator.Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            storage = new Mock<IGetTopicsStorage>();
            getTopicsSetup = storage.Setup(
                s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));


            sut = new GetTopicsUseCase(validator.Object, storage.Object);
        }

        [Fact]
        public async Task ReturnTopics_ExtractedFromStorage()
        {
            var forumId = Guid.Parse("DB8BF4E9-D12C-4793-92E1-710172D4746F");

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
