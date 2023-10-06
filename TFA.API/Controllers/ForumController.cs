using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFA.API.Models;
using TFA.Storage;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ForumModel[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] ForumDbContext context,
            CancellationToken cancellationToken)
        {
            var forums = await context.Forums
                .Select(f => new ForumModel
                {
                    Id = f.ForumId,
                    Title = f.Title
                }).ToArrayAsync();
            return null;
        }
    }
}