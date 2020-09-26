using RPG.Base;
using RPG.Control;
using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Base
{
    public class Interactable : MonoBehaviour, IRaycastable
    {

        public List<ActionMenuOptions> actionMenuOptions;
        public float interactRadius = 3f;
        bool hasInteracted = false;
        public CursorType defaultCursorType;

        public Transform interactionPoint;

        public string displayName;

        Outline objectOutline;

        private void Awake()
        {
            InitializeOutline();
        }

        public void InitializeOutline()
        {
            objectOutline = GetComponent<Outline>();
            objectOutline.enabled = false;
        }

        public virtual void DefaultInteract()
        {
            // This method is meant to be overwritten
            //Debug.Log("Interacting with " + transform.name);
        
        }

         private void OnDrawGizmosSelected() {
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(interactionPoint.position, interactRadius);
         }

        public virtual bool HandleRaycast(PlayerController callingController)
        {
            return true;
        }


        public CursorType GetCursorType(PlayerController callingController)
        {
            return defaultCursorType;
        }

        private void OnMouseEnter()
        {
            InfoToolTip.ShowToolTip_Static(displayName);
            objectOutline.enabled = true;
        }

        private void OnMouseExit()
        {
            InfoToolTip.HideToolTip_Static();
            objectOutline.enabled = false;
        }

#region InteractiveOptions


        public virtual void ShopMenu()
        {

        }


        public virtual void TalkToNPC()
        {
        }

        public virtual void AttackNPC( ControllableObject controllable)
        {

        }

        public virtual void Scan()
        {
            
        }

        public virtual void PickUp(ControllableObject controllable)
        {

        }

    #endregion
    }
}