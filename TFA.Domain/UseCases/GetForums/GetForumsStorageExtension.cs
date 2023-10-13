using TFA.Domain.Exceptions;

namespace TFA.Domain.UseCases.GetForums
{
    internal static class GetForumsStorageExtension
    {
        public static async Task<bool> ForumExist(this IGetForumsStorage storage, Guid forumId, CancellationToken cancellationToken)
        {
            var forums = await storage.GetForums(cancellationToken);
            return forums.Any(f => f.Id == forumId);
        }

        public static async Task ThrowIfForumNotFound(this IGetForumsStorage storage, Guid forumId, CancellationToken cancellationToken)
        {
            if (! await ForumExist(storage, forumId, cancellationToken))
            {
                throw new ForumNotFoundException(forumId);
            }
        }
    }
}
