namespace CursorBased.V4;

public abstract class PaginationHandler<TPaginationContext, TCursor, TItem>
    : Sequential.V2.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    protected override async Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => await ExtractCursorAsync(context) is not null;
    
    protected abstract Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
