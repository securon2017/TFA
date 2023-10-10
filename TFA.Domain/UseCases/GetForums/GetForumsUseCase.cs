using TFA.Domain.ModelsDTO;

namespace TFA.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly IGetForumsStorage _forumsStorage;

        public GetForumsUseCase(IGetForumsStorage forumsStorage)
        {
            _forumsStorage = forumsStorage;
        }

        public Task<IEnumerable<ForumDTO>> Execute(CancellationToken cancellationToken) =>
          _forumsStorage.GetForums(cancellationToken);
    }
}
