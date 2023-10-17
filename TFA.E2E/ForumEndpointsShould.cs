using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TFA.API;
using TFA.API.Models;

namespace TFA.E2E
{
    public class ForumEndpointsShould : IClassFixture<ForumApiApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> factory;

        public ForumEndpointsShould(ForumApiApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task CreateNewForum()
        {
            using var httpclient = factory.CreateClient();
            using var response = await httpclient.PostAsync("forums",
                JsonContent.Create(new { title = "Test" }));
            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            var forum = await response.Content.ReadFromJsonAsync<ForumModel>();
            forum
                .Should().NotBeNull().And.
                Subject.As<ForumModel>().Title.Should().Be("Test");

            using var getForumsResponse = await httpclient.GetAsync("forums");
            var forums = await getForumsResponse.Content.ReadFromJsonAsync<ForumModel[]>();
            forums
                .Should().NotBeNull().And
                .Subject.As<ForumModel[]>().Should().Contain(f=>f.Title == "Test");
        }
    }
}
