using System.Collections.Generic;
using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{

    public class DisplayCrewList : MonoBehaviour
    {
        [SerializeField] private UIController uIController;
        [SerializeField] private GameObject displayMenuOptions;
        [SerializeField] private GameObject menuDisplayContainer;

        private void OnEnable() {
            GenerateOptionsDisplay(displayMenuOptions, menuDisplayContainer, GameEvents.instance.GetCrewRoster());
        }

        private void DoTheThing()
        {
            GenerateOptionsDisplay(displayMenuOptions, menuDisplayContainer, GameEvents.instance.GetCrewRoster());
        }


        private void GenerateOptionsDisplay(GameObject displayMenuOptions, GameObject displayContainer, List<CrewMember> availableCrew)
        {
            RefreshItemDisplayStat();

            foreach (CrewMember crew in availableCrew)
            {
                
            RectTransform displayMenuRectTransform = Instantiate(displayMenuOptions.transform, displayContainer.transform).GetComponent<RectTransform>();

            Text nameToDisplay = displayMenuRectTransform.GetComponentInChildren<Text>();
            Button menuButton = displayMenuRectTransform.GetComponent<Button>();
            

            CrewMember associatedCrewMember = crew;

            menuButton.onClick.AddListener(() => uIController.SetCrewToDisplay(crew));
            menuButton.onClick.AddListener(() => DoTheThing());

            displayMenuRectTransform.gameObject.SetActive(true);

            //displayMenuRectTransform.anchoredPosition = new Vector2(optionsToDisplay.preferredWidth, optionsToDisplay.preferredHeight);

            nameToDisplay.text = crew.GetCrewName();

            if (crew == uIController.GetCrewToDisplay())
            {
               menuButton.GetComponent<Image>().color = menuButton.colors.pressedColor;
            }
        }
        }

        public void RefreshItemDisplayStat()
        {
            foreach (Transform child in menuDisplayContainer.transform)
            {
                if (child == displayMenuOptions.transform)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

    }
}
