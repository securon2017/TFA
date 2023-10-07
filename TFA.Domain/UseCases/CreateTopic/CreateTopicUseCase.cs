using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFA.Domain.Exceptions;
using TFA.Domain.ModelsDTO;
using TFA.Storage;

namespace TFA.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly ForumDbContext _context;
        private readonly IGuidFactory _guidFactory;
        private readonly IMomentProvider _momentProvider;

        public CreateTopicUseCase(ForumDbContext context, 
                                  IGuidFactory guidFactory,
                                  IMomentProvider momentProvider)
        {
            _context = context;
            _guidFactory = guidFactory;
            _momentProvider = momentProvider;
        }
        public async Task<TopicDTO> Execute(Guid forumId, string title, Guid AuthorId, CancellationToken cancellationToken)
        {
            var forumExists = await _context.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
            if(!forumExists) 
            { 
                throw new ForumNotFoundException(forumId); 
            }

            var topicId = _guidFactory.Create();
            await _context.Topics.AddAsync(new Topic
            {
                TopicId = topicId,
                ForumId = forumId,
                Title = title,
                UserId = AuthorId,
                CreatedAt = _momentProvider.Now
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return await _context.Topics
                .Where(t => t.TopicId == topicId)
                .Select(t => new TopicDTO
                {
                    Id = t.TopicId,
                    Title = t.Title,
                    CreatedAt = t.CreatedAt,
                    Author = t.Author.Login
                })
                .FirstAsync(cancellationToken);
        }
    }
}
