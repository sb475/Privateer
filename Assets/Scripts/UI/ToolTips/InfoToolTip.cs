using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
    
    public class InfoToolTip : MonoBehaviour
    {

        private static InfoToolTip instance;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private RectTransform canvasRectTransform;

        [SerializeField] private Text tooltipText;
        [SerializeField] private RectTransform backgroundRectTransform;

        private void Awake() {
            instance = this;
            gameObject.SetActive(false);
           // backgroundRectTransform = transform.Find("ToolTipBackground").GetComponent<RectTransform>();
            //tooltipText = transform.Find("ToolTipText").GetComponent<Text>();            
        }

        private void Update() {
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localpoint);
            transform.localPosition = localpoint;

            Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
            //if the tooltip goes past the edge of our canvas, change the anchored position so that it fits.

            // Debug.Log(anchoredPosition.x + backgroundRectTransform.rect.width);
            // Debug.Log(canvasRectTransform.rect.width);
            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }
            transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }
        // Start is called before the first frame update
    private void ShowToolTip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 10;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string tooltipString) {
        instance.ShowToolTip(tooltipString);

    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();

    }

    }

}