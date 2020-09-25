using RPG.Attributes;
using RPG.Base;
using RPG.Items;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    
    public class CrewMember : MonoBehaviour, IRaycastable
    
    {

        [Header("I'm following")]
        public CrewMember leader;
        public Health health;
        public Mover mov;
        public Inventory inventory;
        public StateManager turnManager;
        public CharacterEquipment equipment;
        public CharacterStats stats;
        public BaseStats baseStats;
        public Rigidbody body;
        private string crewName;
        
        float armor;

        public float followDistance = 5f;
        public bool followLeader;

        private void Awake() {
            health = GetComponent<Health>();
            mov = GetComponent<Mover>();
            inventory = GetComponent<Inventory>();
            turnManager = GetComponent<StateManager>();
            equipment = GetComponent<CharacterEquipment>();
            stats = GetComponent<CharacterStats>();
            baseStats = GetComponent<BaseStats>();
            

            crewName = gameObject.name;
            
        }

        private void Update() {            
            
            if (followLeader)
            {
                if (turnManager.isInCombat)
                {
                    StopFollowingTheLeader();
                }
                FollowTheLeader(leader);
            }
        }


        public string GetCrewName()
        {
            return crewName;
        }
        
        public void MoveToTarget(Vector3 target)
        {
            if (turnManager.canMove) mov.MoveTo(target, 1f);
        }

        public void AddItemToInventory(ItemConfig item, int quantity)
        {
            inventory.AddItem(new ItemInInventory { itemObject = item, itemQuantity = quantity });
        }

        public CursorType GetCursorType(CrewController callingController)
        {
            return CursorType.Interact;
        }

        public bool HandleRaycast(CrewController callingController)
        {
            return true;
        }

        public void FollowTheLeader(CrewMember leader)
        { 
            this.leader = leader;
            if (turnManager.canMove) mov.FollowTarget(leader);
            
        }

        public void StopFollowingTheLeader()
        {
            followLeader = false;
            mov.StopFollowTarget();
        }

        
    }

    

}