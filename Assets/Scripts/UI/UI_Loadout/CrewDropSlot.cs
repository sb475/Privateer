using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RPG.UI{
        
    public class CrewDropSlot : DropContainer
    {

        public enum CrewListType { currentTeam, onShip}

        public CrewListType crewListType;

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log ("Item dropped");
            if (eventData.pointerDrag.GetComponent<CrewSwappableButton>() == null) return;

            Debug.Log("Item was dropped");
            CrewMember crewToSwap = eventData.pointerDrag.GetComponent<CrewSwappableButton>().GetCrewMemberOnObject(); 
            if (crewToSwap == null) return;

            if (eventData.pointerDrag.GetComponentInParent<CrewDropSlot>().GetCrewListType() == CrewListType.currentTeam)
            {
                Debug.Log ("Move crew to ship");
                uIController.DropCrewMember(crewToSwap, uIController.crewController.currentTeam, uIController.crewController.crewOnShip);
            }
            else
            {
                Debug.Log("Move crew to team");
                uIController.DropCrewMember(crewToSwap, uIController.crewController.crewOnShip, uIController.crewController.currentTeam);
            }

        }

        public CrewListType GetCrewListType()
        {
            return crewListType;
        }
    }


}