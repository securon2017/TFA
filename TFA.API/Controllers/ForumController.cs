using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TFA.API.Models;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

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
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);
            return Ok(forums.Select(mapper.Map<ForumModel>));
        }

        [HttpPost("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(TopicViewModel))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {

            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), mapper.Map<TopicViewModel>(topic));
        }


        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(201, Type = typeof(ForumModel))]
        public async Task<IActionResult> CreateForum(
            [FromBody] CreateForum request,
            [FromServices] ICreateForumUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var command = new CreateForumCommand(request.Title);
            var forum = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), mapper.Map<ForumModel>(forum));
        }



        [HttpGet("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(410)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTopics(
            [FromRoute] Guid forumId,
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IGetTopicsUseCase useCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var (resources, totalCount) = await useCase.Execute(new GetTopicsQuery(forumId, skip, take), cancellationToken);
            return Ok(new { resources = resources.Select(mapper.Map<TopicViewModel>) });
        }


    }
}