namespace RPG.Control
{
    public interface IRaycastable
    {
        // Asks Raycastable what type of cursor to give it
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController callingController);
    }
}