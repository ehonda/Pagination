using JetBrains.Annotations;

namespace Sequential.Composite;

// TODO: notnull constraints?
// TODO: Better name for TTransformedPage?
[PublicAPI]
public interface IPageRetriever<in TPaginationContext, TPage>
{
    // TODO: Option type or nullable?
    Task<TPage> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default);
}
