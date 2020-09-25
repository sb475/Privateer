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
        public delegate void SetDefaultAction(CrewMember callingController);
        public SetDefaultAction defaultAction;

        private void Awake()
        {
            
            displayName = gameObject.name;
            interactionPoint = gameObject.transform;
            inventory = new Inventory(null);
            SetDefaultBehavior();
            InitializeOutline();
        }

        public override void DefaultInteract(CrewMember playerController)
        {
            base.DefaultInteract(playerController);

            defaultAction(playerController);
        }

        private void SetDefaultBehavior()
        {
            if (GetComponent<AIController>() != null)
            {
                if (GetComponent<AIController>().GetAttitude() == AttitudeType.Hostile)
                {
                    defaultAction = AttackNPC;
                    defaultCursorType = CursorType.Combat;
                }
            }
            if (GetComponent<Shop>() != null)
            {
                defaultAction = ShopMenu;
                defaultCursorType = CursorType.Shop;
            }
            else if (GetComponent<DialogueSystemTrigger>() != null)
            {
                defaultAction = TalkToNPC;
                defaultCursorType = CursorType.Dialogue;
            }

        }

        public void ShopMenu (CrewMember playerController)
        {
            Shop npcShop = GetComponent<Shop>();
            npcShop.OpenShopMenu();
        }

        public void TalkToNPC(CrewMember playerController)
        {
            if (GetComponent<DialogueSystemTrigger>() == null)
            {
                Debug.Log("I don't have anything to talk about");
                return;
            }
            GetComponent<DialogueSystemTrigger>().OnUse(playerController.transform);
        }

        public void AttackNPC (CrewMember playerController)
        {
            Debug.Log("AttackNPC");
            interactRadius = playerController.GetComponent<Fighter>().currentWeaponConfig.GetRange() * 0.8f;
            playerController.GetComponent<Fighter>().Attack(gameObject);
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