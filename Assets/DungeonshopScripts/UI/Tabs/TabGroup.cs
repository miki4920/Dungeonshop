using System.Collections;
using System.Collections.Generic;
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
            button.background.color = tabHover;
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            ResetTabs();
            button.background.color = tabActive;
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                button.background.color = tabIdle;
            }
        }
    }
}
