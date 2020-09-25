using RPG.Control;
using RPG.Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using RPG.Global;

namespace RPG.UI
{
    public class PerkButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public PerkType perkButtonType;
        private int nextPerkLevelAvailable = 0;

        [SerializeField] private  TextMeshProUGUI nextPerkLevelAvailableDisplay;
        [SerializeField] private TextMeshProUGUI perkTitledisplay;

        private void Awake() {

            nextPerkLevelAvailableDisplay.text = nextPerkLevelAvailable.ToString();
            perkTitledisplay.text = perkButtonType.ToString();
            
        }

        public bool CanBuyPerk()
        {
            return true;
        }
        public int DisplayNextLevelAvailable()
        {
            return nextPerkLevelAvailable += 1;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (CanBuyPerk())
            {
                GameEvents.instance.BuyPerk(perkButtonType, nextPerkLevelAvailable);
                nextPerkLevelAvailableDisplay.text = DisplayNextLevelAvailable().ToString();
            }
            else
            {
                GameEvents.instance.SendMessage("You do not have enough points to spend");
            }


        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //display tooltip
            //ItemToolTip.ShowToolTip_Static(GetComponent<UIItemData>(), uIInventory);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemToolTip.HideToolTip_Static();
        }
    }
}