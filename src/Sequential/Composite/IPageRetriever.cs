namespace Sequential.Composite;

// TODO: notnull constraints?
public interface IPageRetriever<TPage>
{
    // TODO: Option type or nullable?
    Task<TPage> GetAsync(IPaginationContext<TPage>? context, CancellationToken cancellationToken = default);
}
