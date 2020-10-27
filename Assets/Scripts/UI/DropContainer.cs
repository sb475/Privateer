using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class DropContainer : MonoBehaviour, IDropHandler
    {
        public UIController uIController;

        public virtual void Awake() {
            uIController = GetComponentInParent<UIController>();
        }
       
        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.GetComponent<DroppableObject>() == null ) return;

        }

        public void UpdateParent(GameObject droppedObject, GameObject parentToUpdate)
        {
            droppedObject.transform.SetParent(parentToUpdate.transform, false);
            //droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }

    }

}