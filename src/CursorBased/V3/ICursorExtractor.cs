namespace CursorBased.V3;

public interface ICursorExtractor<in TPaginationContext, TCursor>
{
    Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
