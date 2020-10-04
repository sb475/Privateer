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
    [RequireComponent(typeof(IDamagable))]
    [RequireComponent(typeof(AIController))]
    public class CharacterInteraction : Interactable
    {
        public Inventory inventory;
        public IDamagable health;


        public override void Awake()
        {
            base.Awake();
        
            displayName = gameObject.name;
            interactionPoint = gameObject.transform;
            inventory = GetComponent<Inventory>();
            health = GetComponent<IDamagable>();

            InitializeDefaultBehaviour();

        }

        public override void DefaultInteract(ControllableObject callingController)
        {
                if (defaultInteraction == AttackNPC)
                {
                    Debug.Log ("Default was to attack!");
                    interactRadius = callingController.GetComponent<Fighter>().currentWeaponConfig.GetRange();
                    base.DefaultInteract(callingController);
                }
                else
                {
                    base.DefaultInteract(callingController);
                }

        }

        public virtual void InitializeDefaultBehaviour()
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
            controllable.GetComponent<IAttack>().Attack(this.health);
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
            CharacterStats combatTargetStat = GetComponent<CharacterStats>();
            Debug.Log(gameObject.name);

            // Debug.Log(combatTargetStat.GetStat(Stat.Armor));
            // Debug.Log(combatTargetStat.GetStat(Stat.Health));
            Debug.Log(combatTargetStat.GetLevel());
        }

    }
}