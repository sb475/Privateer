using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{

    public class BufferEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Vector3 direction = Vector3.forward;
        [SerializeField] float speed = 12f;
        [SerializeField] GameObject menuImage;
        [SerializeField] GameObject subMenu;

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.rotateAroundLocal(menuImage, direction, 360f, speed).setRepeat(-1);
            LeanTween.alphaCanvas(menuImage.GetComponent<CanvasGroup>(), 1, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.rotateAroundLocal(menuImage, direction, 360f, speed);
            LeanTween.cancelAll(true);

            LeanTween.alphaCanvas(menuImage.GetComponent<CanvasGroup>(), 0, 0.5f);
            subMenu.SetActive(false);
            
        }

    }

}

