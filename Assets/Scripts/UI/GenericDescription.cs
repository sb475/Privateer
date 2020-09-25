using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class GenericDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        //This script can be used to add description to items that might not have it built in
        [TextArea(15, 20)]
        [SerializeField] string description;

        public void OnPointerEnter(PointerEventData eventData)
        {
            InfoToolTip.ShowToolTip_Static(description);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InfoToolTip.HideToolTip_Static();
        }
        
    }
}