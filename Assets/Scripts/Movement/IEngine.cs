using RPG.Base;
using UnityEngine;

namespace RPG.Movement
{
    public interface IEngine
    {
        void StartMoveAction(Vector3 destination, float speedFraction);
        void MoveToInteract(Interactable newTarget);
        void MoveToLocation(Vector3 newTarget);
        void Cancel();
        void KeyMovement();
        void GracefullyStopAnimate();

    }
}