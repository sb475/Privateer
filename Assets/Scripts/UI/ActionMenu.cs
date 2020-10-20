
using RPG.Base;
using RPG.Control;
using RPG.Global;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
    
    public class ActionMenu : MonoBehaviour
    {

        public static ActionMenu instance;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private RectTransform backgroundRectTransform;
        [SerializeField] private GameObject displayMenuOptions;
        [SerializeField] private GameObject menuDisplayContainer;

        private void Awake() {
            instance = this;
            gameObject.SetActive(false);
             
        }

        //The control options for menu is in singleton GameEvents.

        private void GetPosition() {
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localpoint);
            transform.localPosition = localpoint;

            Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;

            //X-Axis
            //if the tooltip goes past the edge of our canvas, change the anchored position so that it fits.
            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }
            //if it goes to far left, move tooltip to the right
            else if (anchoredPosition.x < backgroundRectTransform.rect.width + 40f)
            {

                anchoredPosition.x = anchoredPosition.x + backgroundRectTransform.rect.width;
            }

            //Y-Axis
            ///if window goes too low, move it up
            if (anchoredPosition.y - backgroundRectTransform.rect.height < 0)
            {
                anchoredPosition.y = anchoredPosition.y + backgroundRectTransform.rect.height;
            }

            transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }

        // Start is called before the first frame update
        private void ShowMenuOptions(Interactable interactable)
        {
            GetPosition();
            RefreshItemDisplayStat();
            gameObject.SetActive(true);
            DisplayMenuOptions(interactable, displayMenuOptions, menuDisplayContainer);

            float textPaddingSize = 4f;
            Vector2 backgroundSize = new Vector2(650 + textPaddingSize * 2f, + menuDisplayContainer.GetComponent<RectTransform>().sizeDelta.y + textPaddingSize * 2f);
            backgroundRectTransform.sizeDelta = backgroundSize;

        }

        private void DisplayMenuOptions(Interactable interactable, GameObject displayMenuOptions, GameObject displayContainer)
        {

            foreach (ActionMenuOptions options in interactable.actionMenuOptions)
            {
                GenerateOptionsDisplay(displayMenuOptions, displayContainer, options, interactable);
            }
        }

        private void GenerateOptionsDisplay(GameObject displayMenuOptions, GameObject displayContainer, ActionMenuOptions options, Interactable interactable)
        {
            RectTransform displayMenuRectTransform = Instantiate(displayMenuOptions.transform, displayContainer.transform).GetComponent<RectTransform>();

            Text optionsToDisplay = displayMenuRectTransform.GetComponentInChildren<Text>();

            Button menuButton = displayMenuRectTransform.GetComponent<Button>();
            menuButton.onClick.AddListener(() => GameEvents.instance.crewController.SelectActionMenu(options, interactable));

            displayMenuRectTransform.gameObject.SetActive(true);

            displayMenuRectTransform.anchoredPosition = new Vector2(optionsToDisplay.preferredWidth, optionsToDisplay.preferredHeight);

            optionsToDisplay.text = options.ToString();
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

            private void HideMenuOptions()
        {
            RefreshItemDisplayStat();
            gameObject.SetActive(false);   
        }
        
        public static void ShowMenuOptions_Static(Interactable interactable) {
            instance.ShowMenuOptions(interactable);
        }

        public static void HideMenuOptions_Static()
        {
            instance.HideMenuOptions();
        }

    }

}