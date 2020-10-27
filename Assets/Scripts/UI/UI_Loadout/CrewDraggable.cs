using System;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace RPG.UI
{
    public class CrewDraggable : DroppableObject
    {

        //this needs to be made into a base class for droppedable items.
        public CrewMember crew;
        public CrewSlot parentCrewSlot;


        public override void Awake() {
            base.Awake();

            parentCrewSlot = GetComponentInParent<CrewSlot>();
        }

        public override void UpdateParentComponents()
        {
            base.UpdateParentComponents();
            parentCrewSlot = GetComponentInParent<CrewSlot>();
        }


        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Item dropped on");
            //GetComponentInParent<CrewSlot>().AddCrewToList(eventData.pointerDrag, GetComponentInParent<CrewSlot>());
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