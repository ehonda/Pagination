namespace CursorBased.V2;

// TODO: Interface or base class? Interface seems more natural
public interface IPaginationContext<out TCursor>
{
    TCursor? Cursor { get; }
}
