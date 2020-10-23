using RPG.AI;
using UnityEngine;

namespace RPG.Control
{
    
    public class CrewMember : Character, IRaycastable, IControllable
    {
        private string crewName;
        public PlayerController controller;

        public float followDistance = 5f;
        public bool followLeader;

        public override void Awake() {
            base.Awake();
            controller = GetComponentInParent<PlayerController>();
            crewName = gameObject.name;
            
        }
        public string GetCrewName()
        {
            return crewName;
        }

        public CursorType GetCursorType(PlayerController callingController)
        {
            return CursorType.Interact;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            return true;
        }

        public void GetWeaponOut()
        {
            animator.SetTrigger("readyWeapons");
        }
        public void KeyMovement()
        {
            engine.KeyMovement();
        }

        public void IssueCommand(ManualActions action, GameObject target)
        {
            agent.actionTarget = target;
            SubGoal crewGoal = controller.command.to[action];
            agent.goals.Add(crewGoal, 100);

        }

        public void IssueCommand(ManualActions action, Vector3 target)
        {
            agent.actionDestination = target;
            SubGoal crewGoal = controller.command.to[action];
            agent.goals.Add(crewGoal, 100);

        }
    }

    

}