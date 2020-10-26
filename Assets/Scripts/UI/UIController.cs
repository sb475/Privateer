using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using RPG.Global;
using RPG.Base;
using RPG.Items;
using RPG.Stats;
using RPG.Core;

namespace RPG.UI
{
    public class UIController : MonoBehaviour
    {

        [Header("Player Data")]
        public Ship ship;
        public PlayerController crewController;
        [SerializeField] CrewMember currentCrewToDisplay;
        [SerializeField] List<CrewMember> crewMembersOnTeam;
        [SerializeField] List<CrewMember> crewMembersOnShip;

        [Header("Window Theme")]
        [Tooltip("All the menu and sub-menu windows")]
        public BorderObject[] windowsToApplyTheme;

        public Image windowBackground;
        public Color windowColor;
        public Sprite windowSprite;
        public Sprite borderSprite;

        [Header("Button Themes")]
        public List<ButtonBehaviour> allButtonsToApplyTheme;
        public ButtonBehaviour selectedButton;
        public Vector3 textScale;
        public Vector3 buttonScale;
        public Color buttonActive;
        public Color buttonIdle;
        public Color buttonHover;

        [Header("BattleHud Options")]
        public Image selectTacticDisplay;

        public enum Theme { blue, orange, red, green, purple, pink, custom1, custom2, custom3 };
        [Header("Theme Settings")]
        public Theme theme;
        int themeIndex;
        public GameObject lensDirt;
        public bool useLensDirt = true;
        public TMP_FontAsset guiFont;


        [Header("Character UI Objects")]
        [SerializeField] GameObject characterUI;
        
        [Header("Equipment and Inventory Slots")]
        
        public GameObject UI_item;
        public UIShipCargo playerInventory;
        public DisplayCharacterStats playerEquipmentStats;

        [Header("Event UI Objects")]
        [SerializeField] GameObject eventUI;
        [SerializeField] GameObject shopHud;
        [SerializeField] GameObject dialogueWindow;
        [SerializeField] GameObject battleHud;
        [SerializeField] GameObject messageWindow;

        [Header("Messagine Event Animations")]
        [SerializeField] Vector3 animationScale;
        [SerializeField] Vector3 normalScale;
        [SerializeField] float timeSinceLastEvent = Mathf.Infinity;

        public event EventHandler<EventArgs> OnCrewToDisplayChange;

        internal void DisplayBattleHud()
        {
            if (battleHud.activeSelf == false)
            {
                battleHud.SetActive(true);
            }
            else
            {
                battleHud.SetActive(false);
            }
            
        }

        public event EventHandler<EventArgs> OnDisplayValueChange;

        [Header("Theme Manager")]



        public Color tabIdle;
        public Color tabHover;
        public Color TabActive;


        [SerializeField] float timeToAnimate;
        internal Color pressedButtonColor;


        private void Start() {
            crewMembersOnTeam = crewController.ListCrew();
            crewMembersOnShip = crewController.ListCrewOnShip();
        }


        private void Update()
        {
            timeSinceLastEvent += Time.deltaTime;
            if (timeSinceLastEvent > 3)
            {
                ResetDisplayMessage();
            }
        }

        #region BattleHud Options
        private void OnBattleTacticChange(object sender, GameEvents.BattleTacticOptions e)
        {
            selectTacticDisplay.sprite = e.sprite;
            Debug.Log ("Updating battleTacticSprite");
        }

        private void ActivateBattleHud(object arg1, Vector3 arg2)
        {
            Debug.Log("activate battlehud");
            battleHud.SetActive(true);
        }

#endregion

#region Button Theme Functions

        public void SubscribeButton(ButtonBehaviour button)
        {
            if (allButtonsToApplyTheme == null)
            {
                allButtonsToApplyTheme = new List<ButtonBehaviour>();
            }

            allButtonsToApplyTheme.Add(button);
        }

        public void OnButtonEnter(ButtonBehaviour button)
        {
            if (selectedButton == null || button != selectedButton)
            {
                if (button.scaleText)
                    {
                        LeanTween.scale(button.GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>(), textScale, .2f);
                    }
                    else{
                        LeanTween.scale(button.GetComponent<RectTransform>(), buttonScale, .2f);
                    }
                
                LeanTween.alpha(button.GetComponent<RectTransform>(), 1, 0.5f);
                button.background.color = buttonHover;
            }

        }

        public void OnButtonExit(ButtonBehaviour button)
        {
            ResetButtons(button);
        }

        // public void SelectButton(ButtonBehaviour button)
        // {
        //     OnTabSelected(button);
        // }

        public void OnButtonSelected(ButtonBehaviour button)
        {
            selectedButton = button;
            ResetButtons(button);
            RestoreScale(button);
            button.background.color = buttonActive;


            LeanTween.alpha(button.GetComponent<RectTransform>(), 1, 0.5f);
            if (button.autoDeselect)
            {
                selectedButton = null;
            }
        }

        public void ResetButtons(ButtonBehaviour button)
        {
            if (selectedButton == button) return;

            button.background.color = buttonIdle;
            RestoreScale(button);
            LeanTween.alpha(button.GetComponent<RectTransform>(), .7f, 0.5f);
        }

        private static void RestoreScale(ButtonBehaviour button)
        {
            if (button.scaleText)
            {
                LeanTween.scale(button.GetComponentInChildren<TextMeshProUGUI>().GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), .2f);
            }
            else
            {
                LeanTween.scale(button.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), 0.5f);
            }
        }



        private void SetTheme ()
        {
            SetFont();
            foreach (BorderObject window in windowsToApplyTheme)
            {
                Image borderWindow = window.GetComponent<Image>();
                borderWindow.color = windowColor;
                borderWindow.sprite = borderSprite;

                Image actualWindow = GetComponentInChildren<WindowObject>().GetComponent<Image>();
                Debug.Log(actualWindow.name);
                actualWindow.color = windowColor;
                actualWindow.sprite = windowSprite;
                
            }
        }

        private void SetFont()
        {
            foreach (TextMeshProUGUI text in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.font = Resources.Load<TMP_FontAsset>(guiFont.name);
            }
        }

        #endregion

        #region Character Displays
        public CrewMember GetCrewToDisplay()
        {
            return currentCrewToDisplay;
        }

        public List<CrewMember> GetCrewMembersOnTeam ()
        {
            return crewMembersOnTeam;
        }

        public List<CrewMember> GetCrewMembersOnSip()
        {
            return crewController.crewOnShip;
        }

        public IEnumerator SetCrewToDisplay(CrewMember crewToDisplay)
        {  
            currentCrewToDisplay = crewToDisplay;

            Debug.Log(currentCrewToDisplay);
            OnCrewToDisplayChange?.Invoke(this, EventArgs.Empty);

            yield return null;
            //yield return new WaitUntil(() => UpdateItemsFromEquipped(currentCrewToDisplay.equipment.equipped));


        }

        internal void DropCrewMember(CrewMember crewToSwap, List<CrewMember> recieving, List<CrewMember> losing)
        {
            Debug.Log(crewToSwap.name + " moving to team");
            recieving.Add(crewToSwap);
            losing.Remove(crewToSwap);
            //if (UpdateCrewList != null) UpdateCrewList?.Invoke(this, EventArgs.Empty);

        }

        //Alert other UI elements that display values need to be updated.
        public void UpdateDisplayValues()
        {
            OnDisplayValueChange?.Invoke(this, EventArgs.Empty);
        }

        //when characterToDisplay changes, find that characters equipment and populate display with it
        public bool UpdateItemsFromEquipped(Dictionary<EquipmentSlots, ItemConfig> equippedItems)
        {
            int itemSlotIndex = 0;
            if (itemSlotIndex < equippedItems.Count)
            {
                foreach (KeyValuePair<EquipmentSlots, ItemConfig> equipedItem in equippedItems)
                {
                    Debug.Log(equipedItem.Key + " " + equipedItem.Value);
                    //find and match equipmentSlots with hardcoded slots
                }

            }
            return true;
        }
#endregion

#region General windows manipulation
        public void ShowShopMenu ()
        {
            shopHud.SetActive(true);
        }


        public void ShowSubMenu(GameObject submenu) {
            if (submenu.activeSelf == false)
            {
                submenu.SetActive(true);
            }
            else if (submenu.activeSelf == true)
            {
                submenu.SetActive(false);
            }

            //want to add click graphic where circle spins really fast when clicked on.
        }

        public void OpenMenu(GameObject menu)
        {
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu)
        {
            menu.SetActive(false);
            ItemToolTip.HideToolTip_Static();
        }

        public void ToggleMenu (GameObject menu)
        {
            if (menu.activeSelf == true)
            {
                menu.SetActive(false);
            }
            else if (menu.activeSelf == false)
            {
                menu.SetActive(true);
            
            }
        }

        public void CharacterUISetTab (TabButton button)
        {
            characterUI.GetComponentInChildren<TabGroup>().OnTabSelected(button);

        }

        //makes sure that if another message is sent in that it restarts the timer.
        public void MessageWindowDisplay (string message)
        {
            if (timeSinceLastEvent < 5f)
            {
                ResetDisplayMessage();
                DisplayMessage (message);
            }
            else
            {
                DisplayMessage(message);
            }
        }

        public void DisplayMessage (string message)
        {
            timeSinceLastEvent = 0;
            messageWindow.SetActive(true);
            LeanTween.alphaCanvas(messageWindow.GetComponent<CanvasGroup>(), 1f, .5f).setEaseSpring();
            Text messageText = messageWindow.GetComponentInChildren<Text>();
            messageText.text = message;
            LeanTween.scale(messageText.gameObject, normalScale, 1);
            LeanTween.scale(messageText.gameObject,animationScale, .8f).setEaseSpring();
            LeanTween.scale(messageText.gameObject, normalScale, .8f);

        }

        public void ResetDisplayMessage ()
        {
            messageWindow.GetComponent<CanvasGroup>().alpha = 0;
            Text messageText = messageWindow.GetComponentInChildren<Text>();
            messageText.text = null;
            messageWindow.SetActive(false);
        }
        #endregion
    }
}
