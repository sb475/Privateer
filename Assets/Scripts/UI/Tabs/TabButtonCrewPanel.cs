using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButtonCrewPanel : TabButton
{
    [Header("Tab Group Index")]
    public int index;
    public bool changeColorOnClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.SelectTabByIndex(this, index);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //tabGroup.OnTabEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //tabGroup.OnTabExit(this);
    }

    public override void Start()
    {
        background = GetComponent<Image>();
    }
}
