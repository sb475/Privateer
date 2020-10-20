using RPG.Attributes;
using RPG.Base;
using RPG.Items;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    
    public class CrewMember : ControllableObject, IRaycastable
    
    {

        [Header("I'm following")]
        public CrewMember leader;
        public Inventory inventory;
        public CharacterEquipment equipment;
        public CharacterStats stats;
        public CharacterStats baseStats;
        public Rigidbody body;
        public Animator animator;
        private string crewName;
        
        float armor;

        public float followDistance = 5f;
        public bool followLeader;

        public override void Awake() {
            base.Awake();
            
            inventory = GetComponent<Inventory>();
            equipment = GetComponent<CharacterEquipment>();
            stats = GetComponent<CharacterStats>();
            baseStats = GetComponent<CharacterStats>();
            animator = GetComponent<Animator>();
            

            crewName = gameObject.name;
            
        }

        public override void Update() {  
            base.Update();          
            
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
        
       
        public void AddItemToInventory(ItemConfig item, int quantity)
        {
            inventory.AddItem(new ItemInInventory { itemObject = item, itemQuantity = quantity });
        }

        public CursorType GetCursorType(PlayerController callingController)
        {
            return CursorType.Interact;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            return true;
        }

        public void FollowTheLeader(CrewMember leader)
        { 
            this.leader = leader;
            if (turnManager.canMove) GetComponent<CharacterEngine>().FollowTarget(leader);
            
        }

        public void StopFollowingTheLeader()
        {
            followLeader = false;
            GetComponent<CharacterEngine>().StopFollowTarget();
        }

        public void GetWeaponOut()
        {
            animator.SetTrigger("readyWeapons");
        }

        
    }

    

}