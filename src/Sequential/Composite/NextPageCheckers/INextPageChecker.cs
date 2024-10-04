using JetBrains.Annotations;

namespace Sequential.Composite.NextPageCheckers;


[PublicAPI]
public interface INextPageChecker<in TPaginationContext>
{
    Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default);
}
