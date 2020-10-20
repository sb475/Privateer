namespace RPG.Control

{
    public interface IRaycastable
    {
        CursorType GetCursorType(PlayerController callingController);
        bool HandleRaycast(PlayerController callingController);
    }

}