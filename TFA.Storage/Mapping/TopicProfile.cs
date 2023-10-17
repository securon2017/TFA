using AutoMapper;
using TFA.Domain.ModelsDTO;

namespace TFA.Storage.Mapping
{
    internal class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, TopicDTO>()
                .ForMember(s => s.Id, d => d.MapFrom(t => t.TopicId));
        }
    }
}
