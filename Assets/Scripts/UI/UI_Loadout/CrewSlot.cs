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
        public bool onShip;
        Image image;
        public override void Awake()
        {
            base.Awake();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log ("Crew dropped");
            GameObject droppedObject = eventData.pointerDrag;
            AddCrewToList(droppedObject, this);
            

        }

        public void AddCrewToList(GameObject droppedObject, CrewSlot slotToAddTo)
        {
            CrewDraggable dropped = droppedObject.GetComponent<CrewDraggable>();
            if (dropped == null) return;

            CrewMember crewToSwap = dropped.GetCrewMemberOnObject();
            if (crewToSwap == null) return;

            if (onShip)
            {
                uIController.DropCrewMember(crewToSwap, uIController.crewController.crewOnShip, uIController.crewController.currentTeam);
            }
            else
            {
                uIController.DropCrewMember(crewToSwap, uIController.crewController.currentTeam, uIController.crewController.crewOnShip);
            }
           UpdateParent(dropped.gameObject, slotToAddTo.gameObject);

            crewOnSlot = crewToSwap;
        }
    }

    
}