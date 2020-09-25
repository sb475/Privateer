using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.UI
{

    public class ShowSubMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Vector3 direction = Vector3.forward;
        [SerializeField] float speed = 12f;
        [SerializeField] GameObject menuImage;

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.rotateAroundLocal(menuImage, direction, 360f, speed).setRepeat(-1);
            LeanTween.alphaCanvas(menuImage.GetComponent<CanvasGroup>(), 1, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var seq = LeanTween.sequence();
            //seq.add(LeanTween.rotateAroundLocal(menuImage, direction, 360f, speed));


            //LeanTween.alphaCanvas(menuImage.GetComponent<CanvasGroup>(), 0, 0.5f);
        }

    }

}
