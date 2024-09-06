namespace Sequential.Composite;

// TODO: notnull constraints?
public interface IPageRetriever<in TPaginationContext, TPage>
{
    Task<TPage> GetPageAsync(TPaginationContext context, CancellationToken cancellationToken = default);
}
