using PixelCrushers.DialogueSystem;
using RPG.Attributes;
using RPG.Base;
using RPG.Combat;
using RPG.Events;
using RPG.Items;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(AIController))]
    public class CharacterInteraction : Interactable
    {
        public Inventory inventory;
        public delegate void DefaultInteraction(ControllableObject controllable);
        public DefaultInteraction defaultInteraction;

        public override void Awake()
        {
            base.Awake();
        
            displayName = gameObject.name;
            interactionPoint = gameObject.transform;
            inventory = new Inventory(null);
            InitializeDefaultBehaviour();

        }


        public override void DefaultInteract(ControllableObject controllable)
        {
            defaultInteraction(controllable);
        }
        public void InitializeDefaultBehaviour()
        {
            
               if (GetComponent<AIController>() != null)
            {
                if (GetComponent<AIController>().GetAttitude() == AttitudeType.Hostile)
                {
                    defaultInteraction = AttackNPC;
                    defaultCursorType = CursorType.Combat;
                }
            }
            if (GetComponent<Shop>() != null)
            {
                defaultInteraction = ShopMenu;
                defaultCursorType = CursorType.Shop;
            }
            else if (GetComponent<DialogueSystemTrigger>() != null)
            {
                defaultInteraction = TalkToNPC;
                defaultCursorType = CursorType.Dialogue;
            }

        }

        public override void AttackNPC (ControllableObject controllable)
        {
            controllable.GetComponent<IAttack>().Attack(this.gameObject);
        }

        public override void ShopMenu (ControllableObject controllable)
        {
            Shop npcShop = GetComponent<Shop>();
            npcShop.OpenShopMenu();
        }

        public override void TalkToNPC(ControllableObject controllable)
        {
            if (GetComponent<DialogueSystemTrigger>() == null)
            {
                Debug.Log("I don't have anything to talk about");
                return;
            }
            GetComponent<DialogueSystemTrigger>().OnUse();
        }

        internal void Scan(CrewMember callingController)
        {
            BaseStats combatTargetStat = GetComponent<BaseStats>();
            Debug.Log(gameObject.name);

            Debug.Log(combatTargetStat.GetStat(Stat.Armor));
            Debug.Log(combatTargetStat.GetStat(Stat.Health));
            Debug.Log(combatTargetStat.GetLevel());
        }

    }
}