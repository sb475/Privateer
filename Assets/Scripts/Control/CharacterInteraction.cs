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
        public delegate void DefaultAttack(ControllableObject controllable);
        public DefaultAttack defaultAttack;

        private void Awake()
        {
            
            displayName = gameObject.name;
            interactionPoint = gameObject.transform;
            inventory = new Inventory(null);
            InitializeOutline();
        }


        public override void DefaultInteract()
        {
            if (GetComponent<AIController>() != null)
            {
                if (GetComponent<AIController>().GetAttitude() == AttitudeType.Hostile)
                {
                    defaultAttack = AttackNPC;
                    defaultCursorType = CursorType.Combat;
                }
            }
            if (GetComponent<Shop>() != null)
            {
                ShopMenu();
                defaultCursorType = CursorType.Shop;
            }
            else if (GetComponent<DialogueSystemTrigger>() != null)
            {
                TalkToNPC();
                defaultCursorType = CursorType.Dialogue;
            }

        }

        public override void AttackNPC (ControllableObject controllable)
        {
        }

        public override void ShopMenu ()
        {
            Shop npcShop = GetComponent<Shop>();
            npcShop.OpenShopMenu();
        }

        public override void TalkToNPC()
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

            //     {
            // Fighter rayCaster = callingController.GetComponent<Fighter>();

            // if (!rayCaster.CanAttack(gameObject))
            // {
            //     return false;
            // }

    }
}