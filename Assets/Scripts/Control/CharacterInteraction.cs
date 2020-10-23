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



        }


        public virtual void InitializeDefaultBehaviour()
        {
 
            if (GetComponent<Shop>() != null)
            {
                defaultAction = new RPG_TaskSystem.Task.Trade { interactable = this };
                //defaultInteraction = ShopMenu;
                defaultCursorType = CursorType.Shop;
            }
            else if (GetComponent<DialogueSystemTrigger>() != null)
            {
                defaultAction = new RPG_TaskSystem.Task.Talk { interactable = this };
                defaultCursorType = CursorType.Dialogue;
            }

        }

    }
}