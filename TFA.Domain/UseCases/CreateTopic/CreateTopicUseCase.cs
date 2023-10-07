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

        public CreateTopicUseCase(ForumDbContext context)
        {
            _context = context;
        }
        public async Task<TopicDTO> Execute(Guid forumId, string title, Guid AuthorId, CancellationToken cancellationToken)
        {
            var forumExists = await _context.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
            if(!forumExists) 
            { 
                throw new ForumNotFoundException(forumId); 
            }
            return new TopicDTO();
        }
    }
}
