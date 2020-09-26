using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class DroppableObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UIController uIController;
        public Canvas canvas;
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        public Vector3 lastPosition;

        public RectTransform parentRectTransform;
        public DropContainer parentDropContainer;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            parentRectTransform = GetComponent<RectTransform>();
            parentDropContainer = GetComponentInParent<DropContainer>();
            uIController = GetComponentInParent<UIController>();

        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            lastPosition = gameObject.transform.position;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .6f;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;


            //sets item back to where it came from if not dropped on an ItemSlot
            transform.localPosition = Vector3.zero;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        
    }
}