using FluentAssertions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests.GetTopic
{
    public class CreateTopicCommandValidatorShould
    {
        private readonly CreateTopicCommandValidator sut;
        public CreateTopicCommandValidatorShould()
        {
            sut = new CreateTopicCommandValidator();
        }

        [Fact]
        public void ReturnSuccess_WhenCommandIsValid()
        {
            var actual = sut.Validate(new CreateTopicCommand(Guid.Parse("A7430325-929B-4A85-A7F0-5B9234199791"), "Hello"));
            actual.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new CreateTopicCommand(Guid.Parse("A7430325-929B-4A85-A7F0-5B9234199791"), "Hello");
            yield return new object[] { validCommand with { ForumId = Guid.Empty } };
            yield return new object[] { validCommand with { Title = "     " } };
            yield return new object[] { validCommand with { Title = string.Empty } };
            yield return new object[] { validCommand with { Title = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries," } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public void ReturnSuccess_WhenCommandIsInvalid(
            CreateTopicCommand command)
        {
            var actual = sut.Validate(command);
            actual.IsValid.Should().BeFalse();
        }
    }
}
