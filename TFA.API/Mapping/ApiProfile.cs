using AutoMapper;
using TFA.API.Models;
using TFA.Domain.ModelsDTO;

namespace TFA.API.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<ForumDTO, ForumModel>();
            CreateMap<TopicDTO, TopicViewModel>();
        }
    }
}
