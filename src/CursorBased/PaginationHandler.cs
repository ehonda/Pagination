using Sequential;

namespace CursorBased;

public abstract class PaginationHandler<TPaginationContext, TCursor, TItem>
    : PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    protected override async Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => await ExtractCursorAsync(context) is not null;
    
    protected abstract Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
