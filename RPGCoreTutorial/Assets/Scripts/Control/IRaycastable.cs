
namespace ANM.Control
{
    public enum CursorType
    {
        NONE, MOVEMENT, COMBAT, UI, PICKUP
    }

    public interface IRaycastable
    {
        bool HandleRayCast(PlayerController controller);
        CursorType GetCursorType();
    }
}