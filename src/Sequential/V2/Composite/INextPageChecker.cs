using JetBrains.Annotations;

namespace Sequential.V2.Composite;

[PublicAPI]
public interface INextPageChecker<in TPaginationContext>
{
    Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default);
}
