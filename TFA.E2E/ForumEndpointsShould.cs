using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TFA.API;

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
            using var response = await httpclient.GetAsync("forums");
            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be("[]");
        }

    }
}
