using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class DropContainer : MonoBehaviour, IDropHandler
    {
        public UIController uIController;

        private void Awake() {
            uIController = GetComponentInParent<UIController>();
        }
       
        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<DroppableObject>() == null ) return;

        }   

    }

}