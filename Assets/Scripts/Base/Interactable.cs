using RPG.Base;
using RPG.Control;
using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RPG.Control.RPG_TaskSystem;

namespace RPG.Base
{

    [RequireComponent(typeof(Outline))]
    public class Interactable : MonoBehaviour, IRaycastable
    {
        public ManualActions defaultAct;

        public delegate void DefaultInteraction(ControllableObject controllable);
        public DefaultInteraction defaultInteraction;
        public Task defaultAction;

        public List<ManualActions> actionMenuOptions;
        public float interactRadius = 3f;
        bool hasInteracted = false;
        public CursorType defaultCursorType;
        public Transform interactionPoint;
        public string displayName;
        Outline objectOutline;
        

        public virtual void Awake()
        {
            InitializeOutline();
        }

        public void InitializeOutline()
        {
            objectOutline = GetComponent<Outline>();
            objectOutline.enabled = false;
        }

        public virtual void DefaultInteract(ControllableObject callingController)
        {

            StartCoroutine(MoveToDefault(callingController, () => defaultInteraction(callingController)));
        
        }

        public ManualActions GetDefaultAction ()
        {
            return defaultAct;
        }

        public IEnumerator MoveToDefault(ControllableObject callingController, Action interactableAction)
        {
            //Move into range for action
            yield return new WaitUntil(() => (InRangeToInteract(callingController)));
            interactableAction?.Invoke();
        }


        bool InRangeToInteract(ControllableObject callingController)
        {
            float distanceToInteractable = Vector3.Distance(transform.position, callingController.transform.position);
            if (distanceToInteractable > interactRadius)
            {
                callingController.mov.MoveToInteract(this);
                return false;
                // callingController.MoveToTarget
            }
            else
            {
                return true;
            }
        }


        private void OnDrawGizmosSelected() {
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(transform.position, interactRadius);
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
    }
}