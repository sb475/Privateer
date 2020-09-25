using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOnEnable : MonoBehaviour
{

    public TabGroup tabGroup;
    public TabButton tabButton;

    public virtual void OnEnable() {
        StartCoroutine(EnsureTabSelectDisplay(tabButton));
    }

    public IEnumerator EnsureTabSelectDisplay(TabButton tabButton)
    {
        yield return new WaitForSeconds (.5f);
        tabGroup.OnTabSelected(tabButton);
    }
}
