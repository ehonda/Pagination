namespace CursorBased;

public record PaginationContext<TTransformedPage, TCursor>(TTransformedPage Page, TCursor? Cursor);
