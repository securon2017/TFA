using FluentAssertions;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics
{
    public class GetTopicsQueryValidatorShould
    {
        private readonly GetTopicsQueryValidator sut = new();

        [Fact]
        public void ReturnSuccess_WhenQueryisValid()
        {
            var query = new GetTopicsQuery(Guid.Parse("A6A14828-61C4-4DA0-ABC4-3EC10243B259"), 10, 5);

            sut.Validate(query).IsValid.Should().BeTrue(); 
        }

        public static IEnumerable<object[]> GetInvalidQuery()
        {
            var query = new GetTopicsQuery(Guid.Parse("A6A14828-61C4-4DA0-ABC4-3EC10243B259"), 10, 5);

            yield return new object[] { query with { ForumId = Guid.Empty } };
            yield return new object[] { query with { Skip = -40 } };
            yield return new object[] { query with { Take = -4 } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidQuery))]
        public void ReturnFailure_WhenQueryIsInValid(GetTopicsQuery query)
        {
            sut.Validate(query).IsValid.Should().BeFalse();
        }
    }
}
