using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonshop.UI
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;
        public TabButton selectedTab;
        public List<GameObject> objectsToSwap;
        public TMP_Text tabGroupName;

        public void Start()
        {
            selectedTab = tabButtons[0];
            int index = tabButtons[0].transform.GetSiblingIndex();
            selectedTab.background.color = tabActive;
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
            tabGroupName.text = selectedTab.tabName;
        }
        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            tabGroupName.text = button.tabName;
            if (selectedTab == null || button != selectedTab)
            {
                button.background.color = tabHover;
            }
            
            
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.color = tabActive;
            int index = button.transform.GetSiblingIndex();
            for(int i = 0; i <objectsToSwap.Count; i++)
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

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }
                button.background.color = tabIdle;
            }
        }
    }
}
