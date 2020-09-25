using System;
using RPG.Control;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CrewSwappableButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {

        //this needs to be made into a base class for droppedable items.

        [SerializeField] private Canvas canvas;
        private Vector3 lastPosition;
        public CrewMember crew;
        public CrewSwap crewSwap;
        CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            crewSwap = GetComponentInParent<CrewSwap>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void SetCrewOnObject(CrewMember crewToSet)
        {
            crew = crewToSet;
        }

        public CrewMember GetCrewMemberOnObject()
        {
            return crew;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            

            lastPosition = gameObject.transform.position;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .6f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;


            //sets item back to where it came from if not dropped on an ItemSlot
            transform.localPosition = Vector3.zero;

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}