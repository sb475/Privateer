using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RPG.UI{
        
    public class CrewSwapContainer : DropContainer
    {

        public enum CrewListType { currentTeam, onShop}

        public CrewListType crewListType;

        public override void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<CrewSwappableButton>() == null) return;

            Debug.Log("Item was dropped");
            CrewMember crewToSwap = eventData.pointerDrag.GetComponent<CrewSwappableButton>().GetCrewMemberOnObject(); 
            if (crewToSwap == null) return;

            if (eventData.pointerDrag.GetComponentInParent<CrewSwapContainer>().GetCrewListType() == CrewListType.currentTeam)
            {
                Debug.Log ("Move crew to ship");
                GameEvents.instance.MoveCrewToShip(crewToSwap);
            }
            else
            {
                Debug.Log("Move crew to team");
                GameEvents.instance.MoveCrewToCurrent(crewToSwap);
            }

        }

        public CrewListType GetCrewListType()
        {
            return crewListType;
        }
    }


}