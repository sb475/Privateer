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

        public virtual void DefaultInteract(CrewMember callingController)
        {
            // This method is meant to be overwritten
            //Debug.Log("Interacting with " + transform.name);
        
        }

         private void OnDrawGizmosSelected() {
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(interactionPoint.position, interactRadius);
         }

        public virtual bool HandleRaycast(CrewController callingController)
        {
            return true;
        }

        private void CanInteract(CrewMember callingController)
        {
            ActionMenu.HideMenuOptions_Static();

            float distanceToItem = Vector3.Distance(callingController.transform.position, transform.position);
            if (distanceToItem > interactRadius)
            {
                Debug.Log("You are too far away to interact with this item");
                callingController.mov.MoveToInteract(this);
                // callingController.MoveToTarget
            }
            else if (distanceToItem < interactRadius)

                DefaultInteract(callingController);
            //StartCoroutine(HideForSeconds(respawnTime));
        }

        public CursorType GetCursorType(CrewController callingController)
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
    }
}