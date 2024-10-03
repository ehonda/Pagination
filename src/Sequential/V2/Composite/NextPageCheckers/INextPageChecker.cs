using JetBrains.Annotations;

namespace Sequential.V2.Composite.NextPageCheckers;


[PublicAPI]
public interface INextPageChecker<in TPaginationContext>
{
    Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default);
}
