using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonshop.UI
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons = new List<TabButton>();
        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;
        public TabButton selectedTab;
        public TMP_Text tabGroupName;
        public Enum tabEnum;
        public Func<TabButton> tabButtonFunction;


        public void Subscribe(TabButton tabButton)
        {
            tabButtons.Add(tabButton);
            if(selectedTab != null)
            {
                OnTabSelected(selectedTab);
            }
            
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

        public void OnTabExit()
        {
            ResetTabs();
            if (selectedTab != null && selectedTab.child == null)
            {
                tabGroupName.text = selectedTab.tabName;
            }
            else if(selectedTab.child != null)
            {
                selectedTab.child.OnTabExit();
            }
        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.color = tabActive;
            tabGroupName.text = selectedTab.tabName;
            foreach(TabButton tabButton in tabButtons)
            {
                if(tabButton.associatedObject != null)
                {
                    tabButton.associatedObject.SetActive(false);
                }
            }
            if (button.associatedObject != null)
            {
                button.associatedObject.SetActive(true);
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
