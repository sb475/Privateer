using System;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CrewSwappableButton : DroppableObject
    {

        //this needs to be made into a base class for droppedable items.

        public CrewMember crew;
        public CrewDropSlot crewSwap;


        public override void Awake() {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            crewSwap = GetComponentInParent<CrewDropSlot>();
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