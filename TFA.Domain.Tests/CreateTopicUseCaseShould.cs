using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;

namespace TFA.Domain.Tests
{
    public class CreateTopicUseCaseShould
    {
        private readonly CreateTopicUseCase _sut;
        private readonly ForumDbContext _forumDbContext;
        private readonly ISetup<IGuidFactory, Guid> createIdSetup;
        private readonly ISetup<IMomentProvider, DateTimeOffset> getNowSetup;


        public CreateTopicUseCaseShould()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));

            _forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);

            var guidFactory = new Mock<IGuidFactory>();
            createIdSetup = guidFactory.Setup(f => f.Create());

            var momentProvider = new Mock<IMomentProvider>();
            getNowSetup = momentProvider.Setup(p => p.Now);

            _sut = new CreateTopicUseCase( _forumDbContext, guidFactory.Object, momentProvider.Object);
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

        [Fact]
        public async Task ReturnNewlyCreatedTopic()
        {
            var forumId = Guid.Parse("DD3D80A9-4CEC-4886-8B93-F0BF8782009F");
            var userId = Guid.Parse("5FBD486C-1F9F-4832-B48F-53BE8A2E0E72");
            await _forumDbContext.Forums.AddAsync(new Forum
            {
                ForumId = forumId,
                Title = "Existing forum"
            });

            await _forumDbContext.Users.AddAsync(new User
            {
                UserId = userId,
                Login = "Vasya"
            });
            await _forumDbContext.SaveChangesAsync();

            createIdSetup.Returns(Guid.Parse("08391190-0AAC-42A4-B607-7DCF1EAB9C16"));
            getNowSetup.Returns(new DateTimeOffset(2023, 10, 07, 18, 05, 30, TimeSpan.FromHours(3)));

            var actual = await _sut.Execute(forumId, "Hello world!", userId, CancellationToken.None);
            var allTopics = await _forumDbContext.Topics.ToArrayAsync();
            allTopics.Should().BeEquivalentTo(new[]
            {
                new Topic
                {
                    ForumId = forumId,
                    UserId = userId,
                    Title = "Hello world!"
                }
            }, cfg => cfg.Including(t => t.ForumId).Including(t => t.UserId).Including(t => t.Title));

            actual.Should().BeEquivalentTo(new TopicDTO
            {
                Id = Guid.Parse("08391190-0AAC-42A4-B607-7DCF1EAB9C16"),
                Title = "Hello world!",
                Author = "Vasya",
                CreatedAt = new DateTimeOffset(2023,10,07,18,05,30, TimeSpan.FromHours(3))
            });
        }
    }
}