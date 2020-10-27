using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(GraphicRaycaster))]
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
            droppedObject.transform.SetParent(parentToUpdate.transform);
            droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }

    }

}