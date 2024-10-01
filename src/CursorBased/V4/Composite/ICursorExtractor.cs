namespace CursorBased.V4.Composite;

public interface ICursorExtractor<in TPaginationContext, TCursor>
{
    Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
