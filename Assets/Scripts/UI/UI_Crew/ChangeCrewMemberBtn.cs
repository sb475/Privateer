using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI{
    
    public class ChangeCrewMemberBtn : ButtonBehaviour
    {
        int displayInTeamIndex;
        public bool rightClick;

        public override void Awake()
        {
            base.Awake();
            displayInTeamIndex = 0;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            SelectNextCrewInTeam();

        }

        public void SelectNextCrewInTeam ()
        {
            if (rightClick)
            {
                if (displayInTeamIndex == uIController.GetCrewMembersInTeam().Count - 1)
                {
                    displayInTeamIndex = 0;
                }
                else{
                    displayInTeamIndex ++;
                }
            }
            else
            {
                if (displayInTeamIndex == 0)
                {
                    displayInTeamIndex = uIController.GetCrewMembersInTeam().Count - 1;
                }
                else
                {
                    displayInTeamIndex -= 1;
                }
            }

            StartCoroutine(uIController.SetCrewToDisplay(uIController.GetCrewMembersInTeam()[displayInTeamIndex]));
            Debug.Log(uIController.GetCrewMembersInTeam()[displayInTeamIndex]);
        }

    }

}
