using Microsoft.AspNetCore.Mvc;
using TFA.API.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ForumModel[]))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);
            return Ok(forums.Select(f => new ForumModel
            {
                Id = f.Id,
                Title = f.Title,
            }));
        }
    }
}