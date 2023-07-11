using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> Tabs;
    private Tab _selectedTab;

    private void Start()
    {
        for(int i = 0; i < Tabs.Count; i++)
        {
            var tempTab = Tabs[i];
            tempTab.GetComponent<Button>().onClick.AddListener(() => SelectTab(tempTab));

            if (i == 0)
            {
                tempTab.ActivateTab();
                _selectedTab = tempTab;
            }
            else
            {
                tempTab.DeactivateTab();
            }
        }
    }

    public Tab AddTab(string tabName, GameObject tabWindow, bool firstTab = false)
    {
        Tab newTab = Instantiate(GlobalPrefabContainer.instance.TabPrefab, transform).GetComponent<Tab>();

        if(firstTab)
        {
            newTab.transform.SetAsFirstSibling();
        }

        newTab.SetText(tabName);
        newTab.SetWindow(tabWindow);
        newTab.GetComponent<Button>().onClick.AddListener(() => SelectTab(newTab));

        Tabs.Add(newTab);

        SelectTab(newTab);

        return newTab;
    }

    public void RemoveTab(Tab removedTab)
    {
        Tabs.Remove(removedTab);

        if(Tabs.Count == 0)
        {
            SelectTab(null);
        }
        else if(_selectedTab == removedTab)
        {
            SelectTab(Tabs[0]);
        }

        Destroy(removedTab);
    }

    public void SelectTab(Tab newSlectedTab)
    {
        if (newSlectedTab == _selectedTab) return;

        _selectedTab?.DeactivateTab();
        _selectedTab = newSlectedTab;
        _selectedTab?.ActivateTab();
    }
}
