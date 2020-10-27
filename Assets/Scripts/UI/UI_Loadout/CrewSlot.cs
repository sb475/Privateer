using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI{
        
    public class CrewSlot : DropContainer
    {
        public CrewMember crewOnSlot;
        public CrewDraggable crewDragObj;
        public bool onShip;
        [SerializeField] Sprite defaultImage;
        Sprite image;
        public override void Awake()
        {
            base.Awake();
            if (defaultImage != null) image = GetComponent<Image>().sprite;
        }

        private void Start()
        {
            crewDragObj = GetComponentInChildren<CrewDraggable>();
            if (crewDragObj == null) image = defaultImage;
        }

        public override void OnDrop(PointerEventData eventData)
        {           
            AddCrewToSlot(eventData.pointerDrag);
        }

        public virtual void ResetSlot()
        {
            crewDragObj = null;
            crewOnSlot = null;
        }

        public virtual void AddCrewToSlot(GameObject droppedObject)
        {
            Debug.Log("Adding " + droppedObject + " to " + this.gameObject.name);
            CrewDraggable dropped = droppedObject.GetComponent<CrewDraggable>();
            if (dropped == null) return;

            CrewMember crewToSwap = dropped.GetCrewMemberOnObject();
            if (crewToSwap == null) return;

            dropped.parentCrewSlot.ResetSlot();

            //if there's something in container then swap items.
            if (crewDragObj != null)
            {
                //Debug.Log("Swapping " + crewDragObj + " with " + dropped.parentCrewSlot.gameObject.name);
                dropped.parentCrewSlot.AddCrewToSlot(crewDragObj.gameObject);
                UpdateParent(droppedObject, this.gameObject);
            }
            else
            {
                UpdateParent(droppedObject, this.gameObject);
            }

            if (onShip)
            {
                uIController.DropCrewMember(crewToSwap, uIController.crewController.crewOnShip, uIController.crewController.currentTeam);
            }
            else
            {
                uIController.DropCrewMember(crewToSwap, uIController.crewController.currentTeam, uIController.crewController.crewOnShip);
            }

            dropped.parentCrewSlot = this;
            crewOnSlot = crewToSwap;
            crewDragObj = dropped;
            image = null;

            uIController.displayAvailableCrew.GenerateAvailableCrew();
        }

    }

    
}