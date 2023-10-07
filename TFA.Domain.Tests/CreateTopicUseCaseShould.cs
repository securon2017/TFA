using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;

namespace TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _sut;
        private readonly ForumDbContext _forumDbContext;
        public CreateTopicUseCaseShould()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
            _forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);
            _sut = new CreateTopicUseCase(_forumDbContext);
        }
        [Fact]
        public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
        {
            await _forumDbContext.Forums.AddAsync(new Forum
            {
                ForumId = Guid.Parse("3BC48A69-5600-4C48-BE8E-BEB9E0600D82"),
                Title = "Basic forum"
            });
            await _forumDbContext.SaveChangesAsync();

            var forumId = Guid.Parse("4646944A-E718-46B2-A082-5DC498FCA487");
            var athorId = Guid.Parse("13C97F98-EA54-481B-9F40-BA904169ECB8");

            await _sut.Invoking(s => s.Execute(forumId, "Some Title", athorId, CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();
            
        }
    }
}