/*
 * IRaycastable - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

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