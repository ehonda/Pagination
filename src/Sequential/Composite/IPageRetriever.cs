namespace Sequential.Composite;

// TODO: notnull constraints?
// TODO: Better name for TTransformedPage?
public interface IPageRetriever<in TTransformedPage, TPage>
{
    // TODO: Option type or nullable?
    Task<TPage> GetAsync(TTransformedPage? context, CancellationToken cancellationToken = default);
}
