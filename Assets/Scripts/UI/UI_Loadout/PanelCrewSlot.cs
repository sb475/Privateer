using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelCrewSlot : CrewSlot
{
    public CrewPanel crewPanel;

    public override void ResetSlot()
    {
        base.ResetSlot();
        crewPanel.DeActivatePanel();

    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        crewPanel.ActivatePanel();
    }

}
