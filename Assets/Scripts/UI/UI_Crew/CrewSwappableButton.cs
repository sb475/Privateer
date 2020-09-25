using System;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CrewSwappableButton : MonoBehaviour, IDragHandler
    {
        CrewMember crew;
        CrewSwap crewSwap;

        public void OnDrag(PointerEventData eventData)
        {
            crewSwap = GetComponentInParent<CrewSwap>();
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