using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color TabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        button.background.color = tabIdle;
        tabButtons.Add(button);
    }

    public void OnTabEnter (TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab) 
        {
            button.background.color = tabHover;
            //LeanTween.alpha(button.GetComponent<RectTransform>(), 1, 0.2f);
        }
    }

    public void OnTabExit (TabButton button)
    {
        ResetTabs();              
    }

    public void SelectTab(TabButton button)
    {
        OnTabSelected(button);
    }

    public void OnTabSelected (TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        button.background.color = TabActive;
        //LeanTween.alpha(button.GetComponent<RectTransform>(), 1, 0.2f);

        int index = button.transform.GetSiblingIndex();
        SwapWindow(index);

    }

    private void SwapWindow(int index)
    {
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void SelectTabByIndex(TabButton button, int i)
    {

        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = tabButtons[i];

        selectedTab.Select();

        ResetTabs();
        tabButtons[i].background.color = TabActive;

        SwapWindow(i);

    }
    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab!=null && selectedTab == button) continue;
            button.background.color = tabIdle;
            //LeanTween.alpha(button.GetComponent<RectTransform>(), .7f, 0.5f);
        }
    }
}

