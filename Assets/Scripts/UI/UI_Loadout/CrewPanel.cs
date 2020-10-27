using RPG.UI;
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

    private void Awake()
    {
        panelImage = GetComponent<Image>();

    }
    private void Start()
    {
        uIController = crewSlot.uIController;
        if (panelActive)
        {
            panelImage.color = inactiveColor;
        }

    }

    public void ActivatePanel()
    {

        if (crewSlot.crewOnSlot == null) return;
        Debug.Log("Panel active!");

        uIController.RegisterCrewPanel(this);
        panelImage.color = activeColor;
        //armorEquipSlot.currentArmor = crewSlot.crewOnSlot.equipment.currentArmor;
    }

    public void DeActivatePanel()
    {
        Debug.Log("Panel deactive");
        panelImage.color = inactiveColor;
    }

}
