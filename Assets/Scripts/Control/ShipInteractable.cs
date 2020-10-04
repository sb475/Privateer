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
    [RequireComponent(typeof(AIShipController))]
    public class ShipInteractable : CharacterInteraction
    {

        public override void Awake() {
            base.Awake();
            
        }
        public override void InitializeDefaultBehaviour()
        {
        if (GetComponent<AIShipController>() != null)
            {
                if (GetComponent<AIShipController>().GetAttitude() == AttitudeType.Hostile)
                {
                    defaultInteraction = AttackNPC;
                    defaultCursorType = CursorType.Combat;
                }
            }
            //add board functionanlity.
            //fire grapples
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

    }
}