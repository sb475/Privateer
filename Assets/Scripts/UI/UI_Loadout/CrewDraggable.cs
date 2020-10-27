using System;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CrewDraggable : DroppableObject
    {

        //this needs to be made into a base class for droppedable items.
        public Transform parentToReturnTo; 
        public CrewMember crew;
        public CrewSlot crewSwap;
        Image image;


        public override void Awake() {
            base.Awake();

            parentToReturnTo = parentDropContainer.transform;
        }

        public override void OnDrop(PointerEventData eventData)
        {
            base.OnDrop(eventData);
            GetComponentInParent<CrewSlot>().AddCrewToList(eventData.pointerDrag, GetComponentInParent<CrewSlot>());
        }

        public void SetCrewOnObject(CrewMember crewToSet)
        {
            crew = crewToSet;
        }

        public CrewMember GetCrewMemberOnObject()
        {
            return crew;
        }

    }
}