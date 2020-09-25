namespace RPG.Control

{
    public interface IRaycastable
    {
        CursorType GetCursorType(CrewController callingController);
        bool HandleRaycast(CrewController callingController);
    }

}