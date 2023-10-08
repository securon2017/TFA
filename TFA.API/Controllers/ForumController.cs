using Microsoft.AspNetCore.Mvc;
using TFA.API.Models;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {

        [HttpGet(Name = nameof(GetForums))]
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

        [HttpPost("{forumId:guid}/topics")]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(TopicViewModel))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken cancellationToken)
        {
            try
            {
                var topic = await useCase.Execute(forumId, request.Title, cancellationToken);
                return CreatedAtRoute(nameof(GetForums), new TopicViewModel
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    CreatedAt = topic.CreatedAt
                });
            }
            catch (Exception exception)
            {

                return exception switch
                {
                    IntentionManagerException => Forbid(),
                    ForumNotFoundException => StatusCode(StatusCodes.Status410Gone),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
           
        }

    }
}