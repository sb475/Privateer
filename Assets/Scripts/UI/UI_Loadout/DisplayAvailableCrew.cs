using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{

    public class DisplayAvailableCrew : DropContainer
    {

        List<CrewMember> crewToDisplay = new List<CrewMember>();
        [SerializeField] private GameObject crewDisplayButton;
        [SerializeField] private GameObject crewDisplayContainer;

        private void Start() {
            uIController.displayAvailableCrew = this;
            GenerateAvailableCrew();
            //GameEvents.instance.UpdateCrewList += TriggerCrewListDisplayEvent;
        }

        private void TriggerCrewListDisplayEvent(object sender, EventArgs e)
        {
            GenerateAvailableCrew();
        }

        private void OnEnable() {
            GenerateAvailableCrew();
        }

        public void GenerateAvailableCrew()
        {
                
            
            crewToDisplay = uIController.GetCrewMembersOnSip();

            RefreshItemDisplayStat();

            foreach (CrewMember crew in crewToDisplay)
            {
                //Debug.Log(crew.GetCrewName());
                if (crew == null) continue;

                RectTransform displayMenuRectTransform = Instantiate(crewDisplayButton.transform, crewDisplayContainer.transform).GetComponent<RectTransform>();
                displayMenuRectTransform.gameObject.SetActive(true);
   
                displayMenuRectTransform.GetComponentInChildren<CrewDraggable>().SetCrewOnObject(crew);
               
            }
        }

        public void RefreshItemDisplayStat()
        {
            foreach (Transform child in crewDisplayContainer.transform)
            {

                if (child == crewDisplayButton.transform)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log(this.name + " dropped");
            CrewDraggable dropped = eventData.pointerDrag.GetComponent<CrewDraggable>();
            if (dropped == null) return;

            CrewMember crewToSwap = eventData.pointerDrag.GetComponent<CrewDraggable>().GetCrewMemberOnObject();
            if (crewToSwap == null) return;

            uIController.DropCrewMember(crewToSwap, uIController.crewController.crewOnShip, uIController.crewController.currentTeam);

            dropped.parentCrewSlot.ResetSlot();

            Destroy(eventData.pointerDrag);
            GenerateAvailableCrew();
        }

    }
}
