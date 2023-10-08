using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFA.Domain.ModelsDTO
{
    public class TopicDTO
    {
        public Guid Id { get; set; }

        public Guid ForumId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }

        public DateTimeOffset CreatedAt { get; set; }


    }
}
