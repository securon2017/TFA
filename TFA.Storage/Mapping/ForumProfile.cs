using AutoMapper;
using TFA.Domain.ModelsDTO;

namespace TFA.Storage.Mapping
{
    internal class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<Forum, ForumDTO>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
        }
    }
}
