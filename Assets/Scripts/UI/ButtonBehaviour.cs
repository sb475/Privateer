using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI{
    
    public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public UIController uIController;
        public Button button;
        public Image background;
        public bool autoDeselect;
        public bool scaleText;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            uIController.OnButtonSelected(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            uIController.OnButtonEnter(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            uIController.OnButtonExit(this);
        }

        public virtual void Awake() {
            uIController = GetComponentInParent<UIController>();
            background = GetComponent<Image>();
            button = GetComponent<Button>();
        }



    }

}
