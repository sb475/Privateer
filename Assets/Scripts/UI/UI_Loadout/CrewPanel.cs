using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewPanel : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;
    public bool panelActive = false;
    public Image panelImage;
    public UIController uIController;

    public ArmorEquipSlot armorEquipSlot;
    public PanelCrewSlot crewSlot;
    public CrewSlotDisplay[] slotContainers;

    private void Awake()
    {
        panelImage = GetComponent<Image>();

    }
    private void Start()
    {
        uIController = crewSlot.uIController;
        if (!panelActive)
        {
            panelImage.color = inactiveColor;
        }

    }

    private void ChangePanelColor(Color color)
    {
        Color c = panelImage.color;
        c = color;
        panelImage.color = c;
    }

    public void ActivatePanel()
    {

        if (crewSlot.crewOnSlot == null) return;
        Debug.Log("Panel active!");
        panelImage.color = activeColor;
        panelActive = true;

        Debug.Log(crewSlot.crewOnSlot.GetCrewName());
        uIController.RegisterCrewPanel(this);

        armorEquipSlot.UpdateArmor();
        UpdateSlotDisplay();

    }

    public void DeActivatePanel()
    {
        Debug.Log("Panel deactive");
        panelImage.color = inactiveColor;
    }

    public void UpdateSlotDisplay()
    {
        foreach (CrewSlotDisplay slotDisplay in slotContainers)
        {
            slotDisplay.UpdateCrewSlotDisplay(armorEquipSlot);
        }
    }
}
