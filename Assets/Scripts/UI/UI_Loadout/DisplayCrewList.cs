using System;
using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{

    public class DisplayCrewList : MonoBehaviour
    {
        [SerializeField] private UIController uIController;
        [SerializeField] private GameObject crewDisplayButton;
        [SerializeField] private GameObject crewDisplayContainer;
        public bool displayCrewOnShip;

        private void Start() {
            GenerateAvilableCrew();
            //GameEvents.instance.UpdateCrewList += TriggerCrewListDisplayEvent;
        }

        private void TriggerCrewListDisplayEvent(object sender, EventArgs e)
        {
            GenerateAvilableCrew();
        }

        private void OnEnable() {
            GenerateAvilableCrew();
        }

        public void GenerateAvilableCrew()
        {
                
            List<CrewMember> crewToDisplay = new List<CrewMember>();

            if(displayCrewOnShip)
            {
                crewToDisplay = uIController.GetCrewMembersOnSip();
            }
            else
            {
                 crewToDisplay = uIController.GetCrewMembersOnTeam();
            }

            RefreshItemDisplayStat();
            
            foreach (CrewMember crew in crewToDisplay)
            {
                Debug.Log(crew.GetCrewName());
                if (crew == null) continue;

                RectTransform displayMenuRectTransform = Instantiate(crewDisplayButton.transform, crewDisplayContainer.transform).GetComponent<RectTransform>();
                displayMenuRectTransform.gameObject.SetActive(true);
   
                displayMenuRectTransform.GetComponentInChildren<CrewSwappableButton>().SetCrewOnObject(crew);
               
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

    }
}
