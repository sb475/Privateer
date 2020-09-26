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

        private void Start() {
            GameEvents.instance.UpdateCrewList += TriggerCrewListDisplayEvent;
        }


        private void TriggerCrewListDisplayEvent(object sender, EventArgs e)
        {
            GenerateOptionsDisplay();
        }

        private void OnEnable() {
            GenerateOptionsDisplay();
        }

        public void GenerateOptionsDisplay( )
        {
            List<CrewMember> crewToDisplay;

            if(crewDisplayContainer.GetComponentInChildren<CrewSwapContainer>().GetCrewListType() == CrewSwapContainer.CrewListType.currentTeam)
            {
                Debug.Log(crewDisplayContainer.GetComponentInChildren<CrewSwapContainer>().GetCrewListType());
                crewToDisplay = GameEvents.instance.GetCrewRoster();
                if (crewToDisplay == null) return;
            }
            else
            {
                Debug.Log(crewDisplayContainer.GetComponentInChildren<CrewSwapContainer>().GetCrewListType());
                crewToDisplay = GameEvents.instance.GetShipRoster();
                if (crewToDisplay == null) return;
            }
            
            
            
            RefreshItemDisplayStat();
            
            foreach (CrewMember crew in crewToDisplay)
            {

                Debug.Log (crew.name);
                
                RectTransform displayMenuRectTransform = Instantiate(crewDisplayButton.transform, crewDisplayContainer.transform).GetComponent<RectTransform>();
            
            if (displayMenuRectTransform.GetComponentInChildren<CrewSwappableButton>() != null)
            {
                    TextMeshProUGUI nameToDisplay = displayMenuRectTransform.GetComponentInChildren<TextMeshProUGUI>();
                    displayMenuRectTransform.GetComponentInChildren<CrewSwappableButton>().SetCrewOnObject(crew);
                    nameToDisplay.text = crew.GetCrewName();
            }

            
            
            Button menuButton = displayMenuRectTransform.GetComponent<Button>();
     

            menuButton.onClick.AddListener(() => uIController.SetCrewToDisplay(crew));

            displayMenuRectTransform.gameObject.SetActive(true);

           

            if (crew == uIController.GetCrewToDisplay())
            {
               menuButton.GetComponentInChildren<Image>().color = menuButton.colors.pressedColor;
            }
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
