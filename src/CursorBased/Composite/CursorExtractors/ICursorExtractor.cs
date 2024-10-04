namespace CursorBased.Composite.CursorExtractors;

public interface ICursorExtractor<in TPaginationContext, TCursor>
{
    Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
